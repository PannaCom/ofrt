using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;
using WebOfficeRental.Helpers;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;

namespace WebOfficeRental.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        private officerentalEntities db = new officerentalEntities();
        // GET: News

        public ActionResult AddCategory()
        {
            return View();
        }

        // httppost AddCategory
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCategory(CatVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminAddCat");
            }

            try
            {
                category _new = new category();
                _new.art_cat_name = model.art_cat_name ?? null;
                _new.art_cat_parent_id = model.art_cat_parent_id ?? null;
                db.categories.Add(_new);
                await db.SaveChangesAsync();

                TempData["Updated"] = "Thêm danh mục thành công";
                return RedirectToRoute("AdminAddCat");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm danh mục mới");
                configs.SaveTolog(ex.ToString());
                return View(model);
            }

        }
        
        // partial cat
        public PartialViewResult _lstCatPartial()
        {
            List<LstCat> data = db.categories.Select(x => new LstCat()
            {
                CatId = x.art_cat_id,
                CatName = x.art_cat_name,
                ParentCatId = x.art_cat_parent_id                
            }).OrderBy(x => x.CatName).ToList();

            var presidents = data.Where(x => x.ParentCatId == null).FirstOrDefault();
            SetChildrenCat(presidents, data);
            return PartialView("_lstCatPartial", presidents);
        }

        private void SetChildrenCat(LstCat model, List<LstCat> danhmuc)
        {
            var childs = danhmuc.Where(x => x.ParentCatId == model.CatId).ToList();
            if (childs.Count > 0)
            {
                foreach (var child in childs)
                {
                    SetChildrenCat(child, danhmuc);
                    model.LstCats.Add(child);
                }
            }
        }


        //ListCats
        public ActionResult ListCats(int? pg, string search)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = db.categories.Where(x => x.art_cat_parent_id != null);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.art_cat_name.ToLower().Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.art_cat_name);
            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        //EditCat
        public async Task<ActionResult> EditCat(int? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            category _cat = await db.categories.FindAsync(id);
            if (_cat == null)
            {
                return View(_cat);
            }
            var getCat = new CatVM()
            {
                art_cat_id = _cat.art_cat_id,
                art_cat_name = _cat.art_cat_name,
                art_cat_parent_id = _cat.art_cat_parent_id
            };

            ViewBag.TenCat = _cat.art_cat_name;
            return View(getCat);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCat(CatVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminEditCat", new { id = model.art_cat_id });
            }
            try
            {
                var _cat = await db.categories.FindAsync(model.art_cat_id);
                if (_cat != null)
                {
                    _cat.art_cat_name = model.art_cat_name ?? null;
                    _cat.art_cat_parent_id = model.art_cat_parent_id ?? null;
                    db.Entry(_cat).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["Updated"] = "Cập nhật danh mục thành công";
                }
                return RedirectToRoute("AdminEditCat", new { id = model.art_cat_id });
            }
            catch (Exception ex)
            {
                TempData["Errored"] = "Có lỗi xảy ra khi cập nhật danh mục.";
                configs.SaveTolog(ex.ToString());
                return RedirectToRoute("AdminEditCat", new { id = model.art_cat_id });
            }

        }



        //DeleteCat
        public ActionResult DeleteCat(int? id)
        {
            if (id == null || id == 0 || id == 1)
            {
                return RedirectToRoute("Admin");
            }
            category _cat = db.categories.Find(id);
            if (_cat == null)
            {
                return View();
            }
            return View(_cat);
        }


        [HttpPost, ActionName("DeleteCat")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCatConfirmed(int? id)
        {
            category _cat = await db.categories.FindAsync(id);
            if (_cat == null)
            {
                return View();
            }
            if (_cat.categories1.Count() > 0)
            {
                TempData["Error"] = "Bạn không thể xóa danh mục này. <br /> Danh mục này chứa danh mục con khác. Vui lòng xóa danh mục con trước.";
                return RedirectToRoute("AdminDeleteCat", new { id = _cat.art_cat_id });
            }
            try
            {
                db.categories.Remove(_cat);
                await db.SaveChangesAsync();
                TempData["Deleted"] = "Danh mục đã được xóa khỏi danh sách.";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }

            return RedirectToRoute("AdminListCats");
        }

        //AddNewBlog
        public ActionResult AddNewBlog()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewOffice(OfficeVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminAddOffice");
            }
            try
            {
                long id = 0;
                office _office = new office();
                _office.buiding_id = model.buiding_id ?? null;
                _office.office_name = model.office_name ?? null;
                _office.office_type = model.office_type ?? null;
                _office.office_new_type = model.office_new_type ?? null;
                _office.office_address = model.office_address ?? null;
                _office.office_price_public = model.office_price_public ?? null;
                _office.office_hotmail = model.office_hotmail ?? null;
                _office.office_hotline = model.office_hotline ?? null;
                _office.office_fanpage = model.office_fanpage ?? null;
                _office.office_acreage = model.office_acreage ?? null;
                _office.office_door = model.office_door ?? null;
                _office.office_table = model.office_table ?? null;
                _office.office_photo = model.office_photo ?? null;
                _office.office_photo_slider = model.office_photo_slider ?? null;
                _office.office_other_descriptions = model.office_other_descriptions ?? null;
                _office.office_views = 0;
                _office.office_votes = 0;
                _office.updated_date = DateTime.Now;
                _office.status = true;
                db.offices.Add(_office);
                await db.SaveChangesAsync();

                id = (long)_office.office_id;

                if (model.dichvuvp.Count() > 0)
                {
                    foreach (var item in model.dichvuvp)
                    {
                        var os = new OfficeService();
                        os.office_id = id;
                        os.service_id = item;
                        db.OfficeServices.Add(os);
                        await db.SaveChangesAsync();
                    }
                }

                TempData["Updated"] = "Đã thêm văn phòng mới.";
                return RedirectToRoute("AdminAddOffice");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm văn phòng");
                configs.SaveTolog(ex.ToString());
                return View(model);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}