using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;
using PagedList;
using WebOfficeRental.Helpers;

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

        public ActionResult VanPhong1(int? pg, string ngay, string gia)
        {
            int pageSize = 6;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.offices where q.office_type == 1 && q.status == true select q);
            if (data == null)
            {
                return View(data);
            }

            #region Thêm điều kiện tìm kiếm
            try
            {
                if (ngay == null) ngay = ""; if (gia == null) gia = "";

                int tuGia = 0; int toiGia = 0;
                if (gia != null && gia != "")
                {
                    String[] ar_gia = gia.Split('-');
                    if (ar_gia.Length == 4)
                    {
                        tuGia = Convert.ToInt32(ar_gia[1].ToString()) * 1000000;
                        toiGia = Convert.ToInt32(ar_gia[2].ToString()) * 1000000;
                    }

                    switch (gia)
                    {
                        case "thoathuan":
                            data = data.Where(x => x.office_price_public == -1);
                            break;
                        default:
                            data = data.Where(x => x.office_price_public >= tuGia && x.office_price_public <= toiGia);
                            break;
                    }
                }
                if (ngay != null && ngay != "")
                {
                    ViewBag.ngay = ngay;
                }

                switch (ngay)
                {
                    case "cuhon":
                        data = data.OrderBy(x => x.updated_date);
                        ViewBag.ngay = ngay;
                        break;
                    default:
                        data = data.OrderByDescending(x => x.updated_date);
                        break;
                }
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            #endregion

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult VanPhong2(int? pg, string ngay, string gia)
        {
            int pageSize = 6;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.offices where q.office_type == 2 && q.status == true select q);
            if (data == null)
            {
                return View(data);
            }

            #region Thêm điều kiện tìm kiếm
            try
            {
                if (ngay == null) ngay = ""; if (gia == null) gia = "";

                int tuGia = 0; int toiGia = 0;
                if (gia != null && gia != "")
                {
                    String[] ar_gia = gia.Split('-');
                    if (ar_gia.Length == 4)
                    {
                        tuGia = Convert.ToInt32(ar_gia[1].ToString()) * 1000000;
                        toiGia = Convert.ToInt32(ar_gia[2].ToString()) * 1000000;
                    }

                    switch (gia)
                    {
                        case "thoathuan":
                            data = data.Where(x => x.office_price_public == -1);
                            break;
                        default:
                            data = data.Where(x => x.office_price_public >= tuGia && x.office_price_public <= toiGia);
                            break;
                    }
                    ViewBag.gia = gia;
                }

                switch (ngay)
                {
                    case "cuhon":
                        data = data.OrderBy(x => x.updated_date);                        
                        break;
                    default:
                        data = data.OrderByDescending(x => x.updated_date);
                        break;
                }
                if (ngay != null && ngay != "")
                {
                    ViewBag.ngay = ngay;
                }
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            #endregion

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ToaNha(int? id, string tentoanha, int? pg, string ngay, string gia, string loaivanphong)
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
            
            ViewBag.Tentoanha = _build.bulding_name;
            ViewBag.Id = id;
            ViewBag.url = tentoanha;

            int pageSize = 6;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = (from q in db.offices where q.buiding_id == id && q.status == true select q);
            if (data == null)
            {
                return View(data);
            }

            #region Thêm điều kiện tìm kiếm
            try
            {
                if (ngay == null) ngay = ""; if (gia == null) gia = ""; if (loaivanphong == null) loaivanphong = "";

                int tuGia = 0; int toiGia = 0;
                if (gia != null && gia != "")
                {
                    String[] ar_gia = gia.Split('-');
                    if (ar_gia.Length == 4)
                    {
                        tuGia = Convert.ToInt32(ar_gia[1].ToString()) * 1000000;
                        toiGia = Convert.ToInt32(ar_gia[2].ToString()) * 1000000;
                    }

                    switch (gia)
                    {
                        case "thoathuan":
                            data = data.Where(x => x.office_price_public == -1);
                            break;
                        default:
                            data = data.Where(x => x.office_price_public >= tuGia && x.office_price_public <= toiGia);
                            break;
                    }
                    ViewBag.gia = gia;
                }
                
                switch (loaivanphong)
                {
                    case "vanphongtrongoi":
                        data = data.Where(x => x.office_type == 2);
                        break;
                    default:
                        data = data.Where(x => x.office_type == 1);
                        break;
                }
                if (loaivanphong != null && loaivanphong != "")
                {
                    ViewBag.loaivanphong = loaivanphong;
                }

                switch (ngay)
                {
                    case "cuhon":
                        data = data.OrderBy(x => x.updated_date);
                        break;
                    default:
                        data = data.OrderByDescending(x => x.updated_date);
                        break;
                }
                if (ngay != null && ngay != "")
                {
                    ViewBag.ngay = ngay;
                }
               

            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            #endregion

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
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