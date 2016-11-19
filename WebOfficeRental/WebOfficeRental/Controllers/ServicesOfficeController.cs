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
    public class ServicesOfficeController : Controller
    {
        private officerentalEntities db = new officerentalEntities();

        // GET: ServicesOffice
        public ActionResult ListServicesOffice(int? pg, string search)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.services select q);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.service_name.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.service_name);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        //AddNewServicesOffice
        public ActionResult AddNewServicesOffice()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewServicesOffice(ServerOfficeVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminAddService");
            }
            try
            {
                service _newService = new service();
                _newService.service_name = model.name ?? null;
                db.services.Add(_newService);
                await db.SaveChangesAsync();

                TempData["Updated"] = "Thêm 1 dịch vụ văn phòng mới thành công";
                return RedirectToRoute("AdminAddService");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm dịch vụ văn phòng");
                configs.SaveTolog(ex.ToString());
                return View(model);
            }
        }

        //EditServicesOffice
        public async Task<ActionResult> EditServicesOffice(int? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            service model = await db.services.FindAsync(id);
            if (model == null)
            {
                return View(model);
            }
            var getModel = new ServerOfficeVM()
            {
                Id = model.service_id,
                name = model.service_name
            };

            ViewBag.NameService = model.service_name;
            return View(getModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditServicesOffice(ServerOfficeVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminEditService", new { id = model.Id });
            }
            try
            {
                var _s = await db.services.FindAsync(model.Id);
                if (_s != null)
                {
                    _s.service_name = model.name ?? null;
                    db.Entry(_s).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["Updated"] = "Cập nhật dịch vụ văn phòng.";
                }
                return RedirectToRoute("AdminEditService", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["Errored"] = "Có lỗi xảy ra khi cập nhật.";
                configs.SaveTolog(ex.ToString());
                return RedirectToRoute("AdminEditService", new { id = model.Id });
            }

        }

        //DeleteServicesOffice
        public ActionResult DeleteServicesOffice(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToRoute("Admin");
            }
            service model = db.services.Find(id);
            if (model == null)
            {
                return View();
            }
            return View(model);
        }


        [HttpPost, ActionName("DeleteServicesOffice")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteServicesOfficeConfirmed(int? id)
        {
            service model = await db.services.FindAsync(id);
            if (model == null)
            {
                return View();
            }
            try
            {
                db.services.Remove(model);
                await db.SaveChangesAsync();
                TempData["Deleted"] = "Dịch vụ đã được xóa khỏi danh sách.";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }

            return RedirectToRoute("AdminListServices");
        }

    }
}