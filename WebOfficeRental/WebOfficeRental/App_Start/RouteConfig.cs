using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

            routes.MapRoute(
                "AdminListOffices",
                "admin/list/offices",
                new { controller = "Offices", action = "ListOffices" }
            );

            routes.MapRoute(
               "AdminAddOffice",
               "admin/office/add",
               new { controller = "Offices", action = "AddNewOffice" }
            );

            routes.MapRoute(
               "AdminEditOffice",
               "admin/office/{id}/edit",
               new { controller = "Offices", action = "EditOffice", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "AdminDeleteOffice",
               "admin/office/{id}/delete",
               new { controller = "Offices", action = "DeleteOffice", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              "AdminRestoreOffice",
              "admin/office/{id}/restore",
              new { controller = "Offices", action = "RestoreOffice", id = UrlParameter.Optional }
           );

            #endregion

            #region Quản lý banner quảng cáo
            routes.MapRoute(
                "AdminListBaners",
                "admin/list/baners",
                new { controller = "Baners", action = "ListBaners" }
            );

            routes.MapRoute(
               "AdminAddBaner",
               "admin/baner/add",
               new { controller = "Baners", action = "AddNewBaner" }
            );

            routes.MapRoute(
               "AdminEditBaner",
               "admin/baner/{id}/edit",
               new { controller = "Baners", action = "EditBaner", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "AdminDeleteBaner",
               "admin/baner/{id}/delete",
               new { controller = "Baners", action = "DeleteBaner", id = UrlParameter.Optional }
            );
            #endregion

            #region Văn phòng

            //Search
            routes.MapRoute(
                "SearchOffices",
                "search/van-phong",
                new { controller = "Home", action = "Search1" }
            );

            //Văn phòng cho thuê
            routes.MapRoute(
                "SearchOfficeType1",
                "van-phong/van-phong-cho-thue",
                new { controller = "Home", action = "VanPhong1" }
            );
            //Văn phòng trọn gói
            routes.MapRoute(
                "SearchOfficeType2",
                "van-phong/van-phong-tron-goi",
                new { controller = "Home", action = "VanPhong2" }
            );

            //Tòa nhà
            routes.Add("SearchBuilds", new SeoFriendlyRoute("toa-nha/{tentoanha}-{id}",
                new RouteValueDictionary(
                    new
                    {
                        controller = "Home",
                        action = "ToaNha",
                        id = UrlParameter.Optional,
                        tentoanha = UrlParameter.Optional
                    }),
                new MvcRouteHandler()));

            // Chi tiết văn phòng
            //VanPhongDetail
            routes.Add("RVanPhongDetail", new SeoFriendlyRoute("van-phong/{tentoanha}/{tenvanphong}-{id}", 
                new RouteValueDictionary(
                   new
                   {
                       controller = "Home",
                       action = "VanPhongDetail",
                       id = UrlParameter.Optional,
                       tentoanha = UrlParameter.Optional,
                       tenvanphong = UrlParameter.Optional
                   }), new MvcRouteHandler()));



            #endregion

            #region 404 Notfound
            // 404 not found
            routes.MapRoute(
                "NotFound",
                "{url}",
                new { controller = "Home", action = "NotFoundPage" }
            );
            #endregion

            #region Quản lý danh mục bài viết

            routes.MapRoute(
                "AdminListCats",
                "admin/list/cats",
                new { controller = "News", action = "ListCats" }
            );

            routes.MapRoute(
               "AdminAddCat",
               "admin/cat/add",
               new { controller = "News", action = "AddCategory" }
            );

            routes.MapRoute(
               "AdminEditCat",
               "admin/cat/{id}/edit",
               new { controller = "News", action = "EditCat", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "AdminDeleteCat",
               "admin/cat/{id}/delete",
               new { controller = "News", action = "DeleteCat", id = UrlParameter.Optional }
            );

            // tin tuc blog
            routes.MapRoute(
               "AdminListNewBlogs",
               "admin/list/newblog",
               new { controller = "News", action = "ListNewBlogs" }
           );

            routes.MapRoute(
               "AdminAddNewBlog",
               "admin/newblog/add",
               new { controller = "News", action = "AddNewBlog" }
            );

            routes.MapRoute(
               "AdminEditNewBlog",
               "admin/newblog/{id}/edit",
               new { controller = "News", action = "EditBlog", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "AdminDeleteNewBlog",
               "admin/newblog/{id}/delete",
               new { controller = "News", action = "DeleteBlog", id = UrlParameter.Optional }
            );

            #endregion 

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    public class SeoFriendlyRoute : Route
    {
        public SeoFriendlyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("id"))
                    routeData.Values["id"] = GetIdValue(routeData.Values["id"]);
            }

            return routeData;
        }

        private object GetIdValue(object id)
        {
            if (id != null)
            {
                string idValue = id.ToString();

                var regex = new Regex(@"^(?<id>\d+).*$");
                var match = regex.Match(idValue);

                if (match.Success)
                {
                    return match.Groups["id"].Value;
                }
            }

            return id;
        }
    }
}
