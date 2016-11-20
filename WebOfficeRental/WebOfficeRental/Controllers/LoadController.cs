using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOfficeRental.Models;

namespace WebOfficeRental.Controllers
{
    [AllowAnonymous]
    public class LoadController : Controller
    {
        private officerentalEntities db = new officerentalEntities();
        // GET: Api
        public string getTinhThanh()
        {
            var p = (from q in db.cities where q.provinces != null orderby q.provinces ascending select q.provinces).ToList().Distinct();
            string pro = "<option value=''>--Chọn tỉnh thành--</option>";
            if (pro.Count() > 0)
            {
                foreach (var item in p)
                {
                    string option = string.Format("<option value='{0}'>{1}</option>", item, item);
                    pro += option;
                }
            }

            return pro;
        }

        public string getQuanHuyen(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.cities where q.provinces.Contains(keyword) orderby q.district ascending select q.district);
            string _district = "<option value=''>--Chọn quận huyện--</option>";
            if (p.Count() > 0)
            {
                foreach (var item in p)
                {
                    string option = string.Format("<option value='{0}'>{1}</option>", item, item);
                    _district += option;
                }
            }
            return _district;
        }

        public ActionResult SaveImage()
        {
            bool isSaved = true;
            var fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}images\\photos", Server.MapPath(@"\")));
                        //string strDay = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString()+DateTime.Now.Day.ToString();
                        string strDay = DateTime.Now.ToString("yyyyMMdd");
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), strDay);

                        var _fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, _fileName);
                        file.SaveAs(path);
                        fName = "/images/photos/" + strDay + "/" + _fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                isSaved = false;
                Helpers.configs.SaveTolog(ex.ToString());
            }
            if (isSaved)
            {
                return Json(new { Message = fName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Có lỗi khi lưu tệp tin" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}