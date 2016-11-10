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
    [Authorize(Roles="Administrator")]
    public class MenusController : Controller
    {
        private officerentalEntities db = new officerentalEntities();

        // GET: Menus
        public ActionResult AddNewMenu()
        {
            ViewBag.lstPoMn = configs.ListPositionMenu();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewMenu(MemuVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminAddMenu");
            }
            try
            {
                menu _newMenu = new menu();
                _newMenu.menu_name = model.menu_name ?? null;
                _newMenu.menu_parent_id = model.menu_parent_id ?? null;
                _newMenu.menu_position = model.menu_position ?? null;
                _newMenu.menu_position_index = model.menu_position_index ?? null;
                _newMenu.menu_url = model.menu_url ?? null;
                db.menus.Add(_newMenu);
                await db.SaveChangesAsync();
                
                TempData["Updated"] = "Thêm menu thành công";
                return RedirectToRoute("AdminAddMenu");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm menu");
                return View(model);
            }
        }

        public ActionResult ListMenus(int? pg, string search)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            var data = db.menus.Where(x => x.menu_parent_id != null);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.menu_name.ToLower().Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderBy(x => x.menu_position_index);
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        //EditMenu
        public async Task<ActionResult> EditMenu(int? id)
        {

            if (id == null || id == 0)
            {
                return View();
            }
            menu _menu = await db.menus.FindAsync(id);
            if (_menu == null)
            {
                return View(_menu);
            }
            var getMenu = new MemuVM()
            {
                menu_id = _menu.menu_id,
                menu_name = _menu.menu_name,
                menu_parent_id = _menu.menu_parent_id,
                menu_position = _menu.menu_position,
                menu_position_index = _menu.menu_position_index,
                menu_url = _menu.menu_url
            };

            ViewBag.lstPoMn = configs.ListPositionMenu();
            ViewBag.TenMenu = _menu.menu_name;
            return View(getMenu);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMenu(MemuVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errored"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToRoute("AdminEditMenu", new { id = model.menu_id });
            }
            try
            {
                var _menu = await db.menus.FindAsync(model.menu_id);
                if (_menu != null)
                {
                    _menu.menu_name = model.menu_name ?? null;
                    _menu.menu_parent_id = model.menu_parent_id ?? null;
                    _menu.menu_position = model.menu_position ?? null;
                    _menu.menu_position_index = model.menu_position_index ?? null;
                    _menu.menu_url = model.menu_url ?? null;
                    db.Entry<menu>(_menu).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["Updated"] = "Cập nhật menu thành công";
                }
                return RedirectToRoute("AdminEditMenu", new { id = model.menu_id });
            }
            catch (Exception ex) 
            {
                TempData["Errored"] = "Có lỗi xảy ra khi cập nhật menu.";
                return RedirectToRoute("AdminEditMenu", new { id = model.menu_id });
            }
           
        }

        public PartialViewResult _lstmenutopPartial()
        {
            List<LstMenu> data = db.menus.Select(x => new LstMenu()
            {
                MenuId = x.menu_id,
                MenuName = x.menu_name,
                ParentMenuId = x.menu_parent_id,
                MenuPositionIndex = x.menu_position_index
            }).OrderBy(x=>x.MenuPositionIndex).ToList();

            var presidents = data.Where(x => x.ParentMenuId == null).FirstOrDefault();
            SetChildrenMenu(presidents, data);
            return PartialView("_lstmenutopPartial", presidents);
        }

        private void SetChildrenMenu(LstMenu model, List<LstMenu> danhmuc)
        {
            var childs = danhmuc.Where(x => x.ParentMenuId == model.MenuId).ToList();
            if (childs.Count > 0)
            {
                foreach (var child in childs)
                {
                    SetChildrenMenu(child, danhmuc);
                    model.LstMenus.Add(child);
                }
            }
        }

        //getPositionMenuNew        
        //[HttpPost]
        //public JsonResult getPositionMenuNew(int? pid)
        //{
        //    try
        //    {
        //        int _iPosition = 0;
        //        if (pid == null || pid == 0)
        //        {
        //            _iPosition += 1;
        //        }

        //        var _menu = db.menus.Where(x => x.menu_position == pid).Count();
        //        if (_menu != null)
        //        {
        //            int iMenu = _menu.menus1.Count();
        //            if (iMenu > 0)
        //            {
        //                _iPosition += iMenu + 1;
        //            }
        //            else
        //            {
        //                _iPosition += 1;
        //            }

        //            if (_menu.menu_parent_id == null)
        //            {
        //                _iPosition = 0;
        //                int count =  db.menus.Where(x => x.menu_parent_id == id).Count();
        //                _iPosition += count + 1;
        //            }
        //        }

        //        return Json(_iPosition, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(0, JsonRequestBehavior.AllowGet);
        //    }
        //}


    }
}