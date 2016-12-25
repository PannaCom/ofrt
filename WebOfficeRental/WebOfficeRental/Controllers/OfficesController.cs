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
    public class OfficesController : Controller
    {
        private officerentalEntities db = new officerentalEntities();
        // GET: Offices
        public ActionResult ListOffices(int? pg, string search)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.offices select q);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.office_name.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.updated_date);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddNewOffice()
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
                ViewBag.buiding_id = _office.buiding_id;
                _office.office_name = model.office_name ?? null;
                _office.office_type = model.office_type ?? null;
                ViewBag.office_type = _office.office_type;
                _office.office_new_type = model.office_new_type ?? null;
                _office.office_address = model.office_address ?? null;
                _office.office_price_public = model.office_price_public ?? null;
                _office.office_hotmail = model.office_hotmail ?? null;
                _office.office_hotline = model.office_hotline ?? null;
                _office.office_fanpage = model.office_fanpage ?? null;
                _office.office_acreage = model.office_acreage ?? null;
                //_office.office_door = model.office_door ?? null;
                //_office.office_table = model.office_table ?? null;
                _office.office_photo = model.office_photo ?? null;
                _office.office_photo_slider = model.office_photo_slider ?? null;
                _office.office_other_descriptions = model.office_other_descriptions ?? null;
                _office.office_views = 0;
                _office.office_votes = 0;                
                _office.updated_date = DateTime.Now;
                _office.status = true;
                _office.office_unit = model.donvi ?? null;
                db.offices.Add(_office);
                await db.SaveChangesAsync();

                id = (long)_office.office_id;

                if (model.dichvuvp != null)
                {
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
                }                

                TempData["Updated"] = "Đã thêm văn phòng mới.";
                //return RedirectToRoute("AdminAddOffice");
                return RedirectToRoute("AdminEditOffice", new { id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm văn phòng");
                configs.SaveTolog(ex.ToString());
                return View(model);
            }
        }

        //EditOffice
        public async Task<ActionResult> EditOffice(long? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            office model = await db.offices.FindAsync(id);
            if (model == null)
            {
                return View(model);
            }
            var getModel = new OfficeVM()
            {
                office_id = model.office_id,
                office_name = model.office_name,
                office_type = model.office_type,
                office_new_type = model.office_new_type,
                office_address = model.office_address,
                office_price_public = model.office_price_public,
                office_hotmail = model.office_hotmail,
                office_hotline = model.office_hotline,
                office_fanpage = model.office_fanpage,
                office_acreage = model.office_acreage,
                //office_door = model.office_door,
                //office_table = model.office_table,
                office_photo = model.office_photo,
                office_photo_slider = model.office_photo_slider,
                office_other_descriptions = model.office_other_descriptions,
                buiding_id = model.buiding_id,
                dichvuvp = model.OfficeServices.Select(x=>x.service_id).ToArray(),
                status = model.status,
                donvi = model.office_unit
            };

            ViewBag.OfficeName = model.office_name;
            return View(getModel);
        }

        public ActionResult LoadPhotoOffice(long? id)
        {
            var model = db.offices.Find(id).office_photos.ToList();
            return PartialView("_LoadPhotoOffice", model);
        }

        public ActionResult upanhvanphong(long? office_id, string photo_url, string photo_title, string photo_alt)
        {
            try
            {
                office_photos _anhmoi = new office_photos();
                _anhmoi.office_id = office_id ?? null;
                _anhmoi.photo_url = photo_url ?? null;
                _anhmoi.photo_title = photo_title ?? null;
                _anhmoi.photo_alt = photo_alt ?? null;
                db.office_photos.Add(_anhmoi);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            return RedirectToRoute("AdminEditOffice", new { id = office_id });
        }

        public ActionResult xoa_anh(long? id)
        {
            long? idoffice = 0;
            try
            {
                var photo = db.office_photos.Find(id);
                if (photo != null)
                {
                    idoffice = photo.office_id;
                    db.office_photos.Remove(photo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            return RedirectToRoute("AdminEditOffice", new { id = idoffice });
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOffice(OfficeVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminEditOffice", new { id = model.office_id });
            }
            try
            {
                var _office = await db.offices.FindAsync(model.office_id);
                if (_office != null)
                {
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
                    //_office.office_door = model.office_door ?? null;
                    //_office.office_table = model.office_table ?? null;
                    _office.office_photo = model.office_photo ?? null;
                    _office.office_photo_slider = model.office_photo_slider ?? null;
                    _office.office_other_descriptions = model.office_other_descriptions ?? null;
                    _office.updated_date = DateTime.Now;
                    _office.office_unit = model.donvi ?? null;
                    db.Entry(_office).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    if (model.dichvuvp != null)
                    {
                        // cap nhat dich vu van phong
                        int[] _serviceExist = _office.OfficeServices.Select(x => x.service_id).ToArray();
                        var arraysAreEqual = Enumerable.SequenceEqual(_serviceExist, model.dichvuvp);
                        if (!arraysAreEqual)
                        {
                            if (_serviceExist.Count() > 0)
                            {
                                db.OfficeServices.RemoveRange(_office.OfficeServices);
                            }
                            foreach (var item in model.dichvuvp)
                            {
                                var os = new OfficeService();
                                os.office_id = _office.office_id;
                                os.service_id = item;
                                db.OfficeServices.Add(os);
                                await db.SaveChangesAsync();
                            }
                        }                
                    }                      

                    TempData["Updated"] = "Đã cập nhật thông tin văn phòng.";
                }
                return RedirectToRoute("AdminEditOffice", new { id = model.office_id });
            }
            catch (Exception ex)
            {
                TempData["Errored"] = "Có lỗi xảy ra khi cập nhật.";
                configs.SaveTolog(ex.ToString());
                return RedirectToRoute("AdminEditOffice", new { id = model.office_id });
            }

        }

        //DeleteOffice
        public ActionResult DeleteOffice(long? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            office model = db.offices.Find(id);
            if (model == null)
            {
                return View();
            }
            return View(model);
        }


        [HttpPost, ActionName("DeleteOffice")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOfficeConfirmed(long? id)
        {
            office model = await db.offices.FindAsync(id);
            if (model == null)
            {
                return View();
            }

            if (model.OfficeServices.Count > 0)
            {
                TempData["Errored"] = "Tòa nhà này không thể xóa. Vì đang chứa nhiều dịch vụ.";
                return RedirectToRoute("AdminDeleteOffice", new { id = id });
            }

            try
            {
                model.deleted_date = DateTime.Now;
                model.status = false;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Deleted"] = "Đã xóa văn phòng thành công.";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }

            return RedirectToRoute("AdminListOffices");
        }

        //RestoreOffice       
        public async Task<ActionResult> RestoreOffice(long? id)
        {           
            try
            {
                var _office = await db.offices.FindAsync(id);
                if (_office != null)
                {
                    if (_office.status == false)
                    {
                        _office.status = true;
                        db.Entry(_office).State = System.Data.Entity.EntityState.Modified;
                        await db.SaveChangesAsync();

                        TempData["Updated"] = "Văn phòng đã được khôi phục.";
                    }
                    
                }
                return RedirectToRoute("AdminEditOffice", new { id = id });
            }
            catch (Exception ex)
            {
                TempData["Errored"] = "Có lỗi xảy ra khi khôi phục.";
                configs.SaveTolog(ex.ToString());
                return RedirectToRoute("AdminEditOffice", new { id = id });
            }

        }

        public string getToaNha()
        {
            var p = (from q in db.buildings where q.bulding_name != null orderby q.bulding_name ascending select new { id = q.bulding_id, name = q.bulding_name }).ToList().Distinct();
            string toanha = "";
            if (p.Count() > 0)
            {
                foreach (var item in p)
                {
                    string option = string.Format("<option value='{0}'>{1}</option>", item.id, item.name);
                    toanha += option;
                }
            }

            return toanha;
        }

        public string getDichvuvp()
        {
            var p = (from q in db.services where q.service_name != null orderby q.service_name ascending select new { id = q.service_id, name = q.service_name }).ToList().Distinct();
            string dichvu = "";
            if (p.Count() > 0)
            {
                foreach (var item in p)
                {
                    string option = string.Format("<option value='{0}'>{1}</option>", item.id, item.name);
                    dichvu += option;
                }
            }

            return dichvu;
        }

    }
}