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


    }
}