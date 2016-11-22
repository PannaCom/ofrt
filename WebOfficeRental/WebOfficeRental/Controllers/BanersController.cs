using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOfficeRental.Controllers
{
    public class BanersController : Controller
    {
        // GET: Baners
        public ActionResult ListBaners()
        {
            return View();
        }
    }
}