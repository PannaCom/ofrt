using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;
using PagedList;

namespace WebOfficeRental.Controllers
{
    public class HomeController : Controller
    {
        private officerentalEntities db = new officerentalEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult News()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<menu> GetMenu()
        {
            var menu = (from m in db.menus where m.menu_parent_id == 1 orderby m.menu_position_index select m).ToList();
            return menu;
        }

        public ActionResult LoadMenu()
        {
            var menu = GetMenu();
            string _menu = "";
            foreach (var item in menu)
            {
                var li = string.Format("<li><a href='{0}'>{1}</a>", item.menu_url, item.menu_name);
                if (item.menus1.Count > 0)
                {
                    var ul = "<ul>";
                    foreach (var item2 in item.menus1)
                    {
                        var li2 = string.Format("<li><a href='{0}'>{1}</a></li>", item2.menu_url, item2.menu_name);
                        ul += li2;
                    }
                    ul += "</ul>";
                    li += ul;
                }
                _menu += li + "</li>";
            }
            return PartialView("_MenuPartial", _menu);
        }

        public ActionResult LoadMenuMobile()
        {
            var menu = GetMenu();
            string _menu = "";
            foreach (var item in menu)
            {
                var li = string.Format("<li class='{0}'><a href='{1}'>{2}</a>", item.menus1.Count > 0 ? "menu-item kode-parent-menu" : "", item.menu_url, item.menu_name);
                if (item.menus1.Count > 0)
                {
                    var ul = "<ul class='dl-submenu'>";
                    foreach (var item2 in item.menus1)
                    {
                        var li2 = string.Format("<li><a href='{0}'>{1}</a></li>", item2.menu_url, item2.menu_name);
                        ul += li2;
                    }
                    ul += "</ul>";
                    li += ul;
                }
                _menu += li + "</li>";
            }
            return PartialView("_MenuMobile", _menu);
        }

        public ActionResult LoadOfficeNewHot()
        {
            var model = (from o in db.offices where o.status == true && o.office_new_type == 2 orderby o.updated_date descending select o).ToList();
            return PartialView("_SectionOfficeHot", model);
        }

        public ActionResult LoadAllBuilds()
        {
            var model = (from b in db.buildings select b).ToList();
            return PartialView("_SectionAllBuilds", model);
        }

        public ActionResult LoadCompnay()
        {
            var model = (from c in db.BannerAdvs orderby c.banner_adv_name select c).ToList();
            return PartialView("_SectionCompnay", model);
        }

        [HttpPost]
        public ActionResult RegisterEmail(string inputemail)
        {
            if (inputemail == null) inputemail = "";
            int result = 0;
            var _emailexist = db.register_email.Where(c => c.email_or_phone == inputemail).FirstOrDefault();
            if (_emailexist != null)
            {
                result = 2;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            try
            {
                register_email model = new register_email();
                model.email_or_phone = inputemail ?? null;
                model.date = DateTime.Now;
                db.register_email.Add(model);
                db.SaveChanges();
                result = 1;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WebOfficeRental.Helpers.configs.SaveTolog(ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadOfficeType1()
        {
            var model = (from o in db.offices where o.status == true && o.office_type == 1 orderby o.updated_date descending select o).Take(3).ToList();
            return PartialView("_LoadOfficeType1", model);
        }

        public ActionResult LoadOfficeType2()
        {
            var model = (from o in db.offices where o.status == true && o.office_type == 2 orderby o.updated_date descending select o).Take(3).ToList();
            return PartialView("_LoadOfficeType2", model);
        }

        public ActionResult Search1()
        {

            return View();
        }

        public ActionResult VanPhong1(int? pg)
        {
            int pageSize = 6;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.offices where q.office_type == 1 && q.status == true orderby q.updated_date descending select q).ToList();
            if (data == null)
            {
                return View(data);
            }
           
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult VanPhong2(int? pg)
        {
            int pageSize = 6;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.offices where q.office_type == 2 && q.status == true orderby q.updated_date descending select q).ToList();
            if (data == null)
            {
                return View(data);
            }

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ToaNha(int? id, string tentoanha, int? pg)
        {
            // check id co tồn tại
            if (id == null || id == 0)
            {
                return View();
            }
            // check id có phải là tòa nhà không?
            var _build = (from bu in db.buildings where bu.bulding_id == id select bu).FirstOrDefault();
            if (_build == null)
            {
                return View();
            }

            int pageSize = 6;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.offices where q.buiding_id == id && q.status == true orderby q.updated_date descending select q).ToList();
            if (data == null)
            {
                return View(data);
            }

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult NotFoundPage(string aspxerrorpath)
        {
            if (!string.IsNullOrEmpty(aspxerrorpath))
            {               
                return RedirectToRoute("NotFound");
            }
            return View();
        }


    }
}