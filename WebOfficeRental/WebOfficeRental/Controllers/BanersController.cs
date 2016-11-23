using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;
using PagedList;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebOfficeRental.Helpers;

namespace WebOfficeRental.Controllers
{
    [Authorize]
    public class BanersController : Controller
    {
        private officerentalEntities db = new officerentalEntities();
        // GET: Baners
        public ActionResult ListBaners(int? pg, string search)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from b in db.BannerAdvs select b);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(b => b.banner_adv_name.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.banner_adv_name);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddNewBaner()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewBaner(banerVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminAddBaner");
            }
            try
            {
                BannerAdv _new = new BannerAdv();
                _new.banner_adv_name = model.name ?? null;
                _new.banner_adv_photo = model.photo ?? null;
                _new.Link = model.link ?? null;
                db.BannerAdvs.Add(_new);
                await db.SaveChangesAsync();

                TempData["Updated"] = "Thêm banner quảng cáo mới thành công";
                return RedirectToRoute("AdminAddBaner");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm mới");
                configs.SaveTolog(ex.ToString());
                return View(model);
            }
        }

        public async Task<ActionResult> EditBaner(int? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            BannerAdv _model = await db.BannerAdvs.FindAsync(id);
            if (_model == null)
            {
                return View(_model);
            }
            var getBanner = new banerVM()
            {
                Id = _model.banner_adv_id,
                name = _model.banner_adv_name,
                photo = _model.banner_adv_photo,
                link = _model.Link
            };

            ViewBag.NameBaner = _model.banner_adv_name;
            return View(getBanner);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBaner(banerVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminEditBaner", new { id = model.Id });
            }
            try
            {
                var _model = await db.BannerAdvs.FindAsync(model.Id);
                if (_model != null)
                {
                    _model.banner_adv_name = model.name ?? null;
                    _model.banner_adv_photo = model.photo ?? null;
                    _model.Link = model.link ?? null;
                    db.Entry(_model).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["Updated"] = "Cập nhật baner quảng cáo thành công";
                }
                return RedirectToRoute("AdminEditBaner", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["Errored"] = "Có lỗi xảy ra khi cập nhật.";
                configs.SaveTolog(ex.ToString());
                return RedirectToRoute("AdminEditBaner", new { id = model.Id });
            }

        }

        //DeleteBaner
        public ActionResult DeleteBaner(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            BannerAdv _model = db.BannerAdvs.Find(id);
            if (_model == null)
            {
                return View();
            }
            return View(_model);
        }


        [HttpPost, ActionName("DeleteBaner")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBanerConfirmed(int? id)
        {
            BannerAdv _model = await db.BannerAdvs.FindAsync(id);
            if (_model == null)
            {
                return View();
            }

            try
            {
                db.BannerAdvs.Remove(_model);
                await db.SaveChangesAsync();
                TempData["Deleted"] = "Quảng cáo đã được xóa khỏi danh sách.";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            return RedirectToRoute("AdminListBaners");
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