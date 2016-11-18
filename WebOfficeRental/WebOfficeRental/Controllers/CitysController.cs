using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;
using PagedList;
using Newtonsoft.Json;

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

    }
}