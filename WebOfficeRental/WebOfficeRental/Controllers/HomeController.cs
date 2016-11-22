using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;

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

    }
}