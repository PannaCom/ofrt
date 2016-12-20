using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;

namespace WebOfficeRental.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using(officerentalEntities db = new officerentalEntities()) {
                ViewBag.SoToaNha = db.buildings.Count();
                ViewBag.SoVanPhong = db.offices.Count();
            }             
            return View();
        }
    }
}