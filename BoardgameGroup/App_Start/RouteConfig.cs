using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BoardgameGroup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Ajax",
                url: "Ajax/{action}",
                defaults: new { controller = "Ajax", action = "Index", club = "DSF" }
            );

            routes.MapRoute(
                name: "File",
                url: "File/{action}/{subaction}",
                defaults: new { controller = "FileExport", action = "Index", subaction = "all", club = "DSF" }
                );

            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Main", action = "Frontpage", club = "DSF" }
            );

            routes.MapRoute(
                name: "Club info",
                url: "{club}/{action}/{subaction}",
                defaults: new { controller = "Main", action = "Frontpage", club = UrlParameter.Optional, subaction = UrlParameter.Optional }
            );
        }
    }
}