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
using Microsoft.AspNet.Identity;

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

        // option cat new
        public PartialViewResult _lstOptionCatPartial()
        {
            List<LstCat> data = db.categories.Select(x => new LstCat()
            {
                CatId = x.art_cat_id,
                CatName = x.art_cat_name,
                ParentCatId = x.art_cat_parent_id
            }).OrderBy(x => x.CatName).ToList();

            var presidents = data.Where(x => x.ParentCatId == null).FirstOrDefault();
            SetChildrenCat(presidents, data);
            return PartialView("_lstOptionCatPartial", presidents);
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
        public async Task<ActionResult> AddNewBlog(articlesVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminAddNewBlog");
            }
            try
            {
                article _newModel = new article();
                _newModel.article_name = model.article_name ?? null;
                _newModel.article_description = model.article_description ?? null;
                _newModel.article_content = model.article_content ?? null;
                _newModel.article_type = model.article_type ?? null;
                _newModel.article_slugurl = model.article_slugurl != null ? model.article_slugurl : Helpers.configs.unicodeToNoMark(model.article_name);
                _newModel.article_photo_sm = model.article_photo_sm ?? null;
                _newModel.article_photo_lg = model.article_photo_lg ?? null;
                _newModel.status = model.status;
                _newModel.art_cat_id = model.art_cat_id ?? null;
                _newModel.tags = model.tags ?? null;
                _newModel.updated_date = DateTime.Now;
                string strUserId = User.Identity.GetUserId();
                //HttpContext.Current.User.Identity.GetUserId();
                _newModel.user_id = strUserId ?? null; 
                db.articles.Add(_newModel);
                await db.SaveChangesAsync();
                TempData["Updated"] = "Đã thêm mới bài viết.";
                return RedirectToRoute("AdminAddNewBlog");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm bài viết.");
                configs.SaveTolog(ex.ToString());
                return View(model);
            }
        }

        //ListNewBlogs
        public ActionResult ListNewBlogs(int? pg, string search)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.articles select q);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.article_name.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.updated_date);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        //EditBlog
        public async Task<ActionResult> EditBlog(long? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            article _model = await db.articles.FindAsync(id);
            if (_model == null)
            {
                return View(_model);
            }
            var getArticle = new articlesVM()
            {
                article_id = _model.article_id,
                tags = _model.tags,
                art_cat_id = _model.art_cat_id,
                article_name = _model.article_name,
                article_description = _model.article_description,
                article_content = _model.article_content,
                article_type = _model.article_type,
                article_slugurl = _model.article_slugurl,
                article_photo_sm = _model.article_photo_sm,
                article_photo_lg = _model.article_photo_lg,
                status = (bool)_model.status
            };

            ViewBag.NameBlog = _model.article_name;
            return View(getArticle);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBlog(articlesVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminEditNewBlog", new { id = model.article_id });
            }
            try
            {
                var _model = await db.articles.FindAsync(model.article_id);
                if (_model != null)
                {
                    //logic code
                    _model.article_name = model.article_name ?? null;
                    _model.article_description = model.article_description ?? null;
                    _model.article_content = model.article_content ?? null;
                    _model.article_type = model.article_type ?? null;
                    _model.article_slugurl = model.article_slugurl != null ? model.article_slugurl : Helpers.configs.unicodeToNoMark(model.article_name);
                    _model.article_photo_sm = model.article_photo_sm ?? null;
                    _model.article_photo_lg = model.article_photo_lg ?? null;
                    _model.status = model.status;
                    _model.art_cat_id = model.art_cat_id ?? null;
                    _model.tags = model.tags ?? null;
                    _model.updated_date = DateTime.Now;
                    string strUserId = User.Identity.GetUserId();
                    //HttpContext.Current.User.Identity.GetUserId();
                    _model.user_id = strUserId ?? null; 
                    db.Entry(_model).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["Updated"] = "Cập nhật bài viết thành công";
                }
                return RedirectToRoute("AdminEditNewBlog", new { id = model.article_id });
            }
            catch (Exception ex)
            {
                TempData["Errored"] = "Có lỗi xảy ra khi cập nhật bài viết.";
                configs.SaveTolog(ex.ToString());
                return RedirectToRoute("AdminEditNewBlog", new { id = model.article_id });
            }

        }

        //DeleteBlog
        public ActionResult DeleteBlog(long? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            article _model = db.articles.Find(id);
            if (_model == null)
            {
                return View();
            }
            return View(_model);
        }


        [HttpPost, ActionName("DeleteBlog")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBlogConfirmed(long? id)
        {
            article _model = await db.articles.FindAsync(id);
            if (_model == null)
            {
                return View();
            }
            try
            {
                _model.status = false;
                _model.deleted_date = DateTime.Now;
                db.Entry(_model).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Deleted"] = "Bài viết đã xóa khỏi danh sách đã đăng.";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            return RedirectToRoute("AdminListNewBlogs");
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