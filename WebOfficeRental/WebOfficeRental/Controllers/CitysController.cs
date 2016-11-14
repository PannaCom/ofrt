using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;

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


    }
}