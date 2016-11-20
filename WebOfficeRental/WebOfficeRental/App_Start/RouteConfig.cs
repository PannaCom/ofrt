using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebOfficeRental
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Admin",
               "admin/dashboard",
               new { controller = "Admin", action = "Index" }
             );

            routes.MapRoute(
              "LoginAccount",
              "admin/login",
              new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
              "UpdateAccount",
              "admin/profile/update",
              new { controller = "Manage", action = "UserProfile" }
            );

            routes.MapRoute(
                "ChangePasswordAccount",
                "admin/profile/changepass",
                new { controller = "Manage", action = "ChangePassword" }
            );

            #region Quản lý menu
            routes.MapRoute(
                "AdminListMenus",
                "admin/list/menus",
                new { controller = "Menus", action = "ListMenus" }
            );

            routes.MapRoute(
               "AdminAddMenu",
               "admin/menu/add",
               new { controller = "Menus", action = "AddNewMenu" }
            );

            routes.MapRoute(
               "AdminEditMenu",
               "admin/menu/{id}/edit",
               new { controller = "Menus", action = "EditMenu", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "AdminDeleteMenu",
               "admin/menu/{id}/delete",
               new { controller = "Menus", action = "DeleteMenu", id = UrlParameter.Optional }
            );



            //routes.MapRoute(
            //    "AGetPositionMenuNew",
            //    "admin/menu/getpositionmenu",
            //    new { controller = "Menus", action = "getPositionMenuNew" }
            //);

            #endregion

            #region Quản lý địa điểm: quận huyện thành phố/tỉnh thành
            routes.MapRoute(
                "AdminListCitys",
                "admin/list/citys",
                new { controller = "Citys", action = "ListCitys" }
            );

            routes.MapRoute(
               "AdminAddCity",
               "admin/city/add",
               new { controller = "Citys", action = "AddNewCity" }
            );

            routes.MapRoute(
               "AdminEditCity",
               "admin/city/{id}/edit",
               new { controller = "Citys", action = "EditCity", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "AdminDeleteCity",
               "admin/city/{id}/delete",
               new { controller = "Citys", action = "DeleteCity", id = UrlParameter.Optional }
            );



            //routes.MapRoute(
            //    "AGetPositionMenuNew",
            //    "admin/menu/getpositionmenu",
            //    new { controller = "Menus", action = "getPositionMenuNew" }
            //);

            #endregion

            #region Quản lý dịch vụ văn phòng
            routes.MapRoute(
                "AdminListServices",
                "admin/list/servicesoffice",
                new { controller = "ServicesOffice", action = "ListServicesOffice" }
            );

            routes.MapRoute(
               "AdminAddService",
               "admin/servicesoffice/add",
               new { controller = "ServicesOffice", action = "AddNewServicesOffice" }
            );

            routes.MapRoute(
               "AdminEditService",
               "admin/serviceoffice/{id}/edit",
               new { controller = "ServicesOffice", action = "EditServicesOffice", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "AdminDeleteServices",
               "admin/servicesoffice/{id}/delete",
               new { controller = "ServicesOffice", action = "DeleteServicesOffice", id = UrlParameter.Optional }
            );
            #endregion

            #region Quản lý tòa nhà (building)
            routes.MapRoute(
                "AdminListBuilds",
                "admin/list/builds",
                new { controller = "Builds", action = "ListBuilds" }
            );

            routes.MapRoute(
               "AdminAddBuild",
               "admin/build/add",
               new { controller = "Builds", action = "AddNewBuild" }
            );

            routes.MapRoute(
               "AdminEditBuild",
               "admin/build/{id}/edit",
               new { controller = "Builds", action = "EditBuild", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "AdminDeleteBuild",
               "admin/build/{id}/delete",
               new { controller = "Builds", action = "DeleteBuild", id = UrlParameter.Optional }
            );
            #endregion

            #region Quản lý văn phòng (offices)



            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
