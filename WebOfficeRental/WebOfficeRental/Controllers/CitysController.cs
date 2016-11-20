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
    public class CitysController : Controller
    {
        private officerentalEntities db = new officerentalEntities();

        // GET: Citys
        public ActionResult AddNewCity()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewCity(CityVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminAddCity");
            }
            try
            {
                city _newCity = new city();
                _newCity.district = model.district ?? null;
                _newCity.provinces = model.provinces ?? null;
                db.cities.Add(_newCity);
                await db.SaveChangesAsync();

                TempData["Updated"] = "Thêm địa chỉ mới thành công";
                return RedirectToRoute("AdminAddCity");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm menu");
                configs.SaveTolog(ex.ToString());
                return View(model);
            }
        }


        public ActionResult ListCitys(int? pg, string search)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.cities select q);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x=>x.district.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.district);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        //getListQuanHuyen, para: keyword
        public string getListQuanHuyen(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.cities where q.district.Contains(keyword) orderby q.district ascending select q.district);
            return JsonConvert.SerializeObject(p.ToList());
        }

        //getListTinhThanh, para: keyword
        public string getListTinhThanh(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.cities where q.provinces.Contains(keyword) orderby q.provinces ascending select q.provinces).ToList().Distinct();
            return JsonConvert.SerializeObject(p);
        }

        //EditCity
        public async Task<ActionResult> EditCity(int? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            city _city = await db.cities.FindAsync(id);
            if (_city == null)
            {
                return View(_city);
            }
            var getCity = new CityVM()
            {
                city_id = _city.city_id,
                district =_city.district,
                provinces = _city.provinces
            };
          
            ViewBag.disTrict = _city.district + " - " + _city.provinces;
            return View(getCity);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCity(CityVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminEditCity", new { id = model.city_id });
            }
            try
            {
                var _city = await db.cities.FindAsync(model.city_id);                
                if (_city != null)
                {
                    _city.district = model.district ?? null;
                    _city.provinces = model.provinces ?? null;
                    db.Entry(_city).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["Updated"] = "Cập nhật đia chỉ thành công";
                }
                return RedirectToRoute("AdminEditCity", new { id = model.city_id });
            }
            catch (Exception ex)
            {
                TempData["Errored"] = "Có lỗi xảy ra khi cập nhật địa chỉ.";
                configs.SaveTolog(ex.ToString());
                return RedirectToRoute("AdminEditCity", new { id = model.city_id });
            }

        }

        //DeleteCity
        public ActionResult DeleteCity(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            city _city = db.cities.Find(id);
            if (_city == null)
            {
                return View();
            }
            return View(_city);
        }


        [HttpPost, ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCityConfirmed(int? id)
        {
            city _city = await db.cities.FindAsync(id);
            if (_city == null)
            {
                return View();
            }

            if (_city.buildings.Count > 0)
            {
                TempData["Errored"] = "Địa chỉ này không thể xóa.";
                return RedirectToRoute("AdminDeleteCity", new { id = id });
            }

            try
            {
                db.cities.Remove(_city);
                await db.SaveChangesAsync();
                TempData["Deleted"] = "Địa chỉ đã được xóa khỏi danh sách.";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }

            return RedirectToRoute("AdminListCitys");
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