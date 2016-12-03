using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;
using WebOfficeRental.Helpers;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;

namespace WebOfficeRental.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        private officerentalEntities db = new officerentalEntities();
        // GET: News

        public ActionResult AddCategory()
        {
            return View();
        }
    }
}