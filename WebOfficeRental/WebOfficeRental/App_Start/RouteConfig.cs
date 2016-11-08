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

            routes.MapRoute(
                "AdminListMenus",
                "admin/list/menus",
                new { controller = "Menus", action = "ListMenus" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
