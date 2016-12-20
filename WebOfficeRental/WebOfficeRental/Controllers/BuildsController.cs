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
    public class BuildsController : Controller
    {
        private officerentalEntities db = new officerentalEntities();
        // GET: Builds
        public ActionResult ListBuilds(int? pg, string search)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.buildings select q);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.bulding_name.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.bulding_name);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddNewBuild()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewBuild(BuildsVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminAddBuild");
            }
            try
            {
                building _newBuild = new building();
                _newBuild.bulding_name = model.bulding_name ?? null;
                _newBuild.provinces = model.provinces ?? null;
                _newBuild.district = model.district ?? null;
                _newBuild.city_id = model.city_id ?? null;
                _newBuild.building_address = model.building_address ?? null;
                _newBuild.building_picture = model.building_picture ?? null;
                _newBuild.building_phonenumber = model.building_phonenumber ?? null;
                _newBuild.building_email = model.building_email ?? null;
                _newBuild.building_fanpage = model.building_fanpage ?? null;
                _newBuild.building_latlong = model.building_latlong ?? null;
                _newBuild.building_description = model.building_description ?? null;
                db.buildings.Add(_newBuild);
                await db.SaveChangesAsync();

                TempData["Updated"] = "Đã thêm thông tin tòa nhà.";
                return RedirectToRoute("AdminAddBuild");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm thông tin tòa nhà");
                configs.SaveTolog(ex.ToString());
                return View(model);
            }
        }

        //EditBuild
        public async Task<ActionResult> EditBuild(int? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            building model = await db.buildings.FindAsync(id);
            if (model == null)
            {
                return View(model);
            }
            var getModel = new BuildsVM()
            {
                bulding_id = model.bulding_id,
                bulding_name = model.bulding_name,
                city_id = model.city_id,
                provinces = model.provinces,
                district = model.district,
                building_address = model.building_address,
                building_picture = model.building_picture,
                building_phonenumber = model.building_phonenumber,
                building_email = model.building_email,                
                building_fanpage = model.building_fanpage,
                building_latlong = model.building_latlong,
                building_description = model.building_description
            };

            ViewBag.BuildName = model.bulding_name;
            return View(getModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBuild(BuildsVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminEditBuild", new { id = model.bulding_id });
            }
            try
            {
                var _b = await db.buildings.FindAsync(model.bulding_id);
                if (_b != null)
                {
                    _b.bulding_name = model.bulding_name ?? null;
                    _b.provinces = model.provinces ?? null;
                    _b.district = model.district ?? null;
                    _b.city_id = model.city_id ?? null;
                    _b.building_address = model.building_address ?? null;
                    _b.building_picture = model.building_picture ?? null;
                    _b.building_email = model.building_email ?? null;
                    _b.building_phonenumber = model.building_phonenumber ?? null;
                    _b.building_fanpage = model.building_fanpage ?? null;
                    _b.building_latlong = model.building_latlong ?? null;
                    _b.building_description = model.building_description ?? null;
                    db.Entry(_b).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["Updated"] = "Đã cập nhật thông tin tòa nhà.";
                }
                return RedirectToRoute("AdminEditBuild", new { id = model.bulding_id });
            }
            catch (Exception ex)
            {
                TempData["Errored"] = "Có lỗi xảy ra khi cập nhật.";
                configs.SaveTolog(ex.ToString());
                return RedirectToRoute("AdminEditBuild", new { id = model.bulding_id });
            }

        }

        //DeleteBuild
        public ActionResult DeleteBuild(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            building model = db.buildings.Find(id);
            if (model == null)
            {
                return View();
            }
            return View(model);
        }


        [HttpPost, ActionName("DeleteBuild")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBuildConfirmed(int? id)
        {
            building model = await db.buildings.FindAsync(id);
            if (model == null)
            {
                return View();
            }

            if (model.offices.Count > 0)
            {
                TempData["Errored"] = "Tòa nhà này không thể xóa. Vì đang chứa nhiều văn phòng.";
                return RedirectToRoute("AdminDeleteBuild", new { id = id });
            }

            try
            {
                db.buildings.Remove(model);
                await db.SaveChangesAsync();
                TempData["Deleted"] = "Tòa nhà đã được xóa.";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }

            return RedirectToRoute("AdminListBuilds");
        }

        public int getCityId(string tinh, string huyen)
        {
            if (tinh == null) tinh = "";
            if (huyen == null) huyen = "";
            var p = (from q in db.cities where q.provinces.Contains(tinh) && q.district.Contains(huyen) select q.city_id).FirstOrDefault();
            return p;
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