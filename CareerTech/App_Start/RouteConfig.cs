using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CareerTech
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "companyPage",
              url: "company/{id}",
              defaults: new { controller = "Partner", action = "IntroductionCompany" }
          );

            routes.MapRoute(
              name: "PortfolioPage",
              url: "portfolio/{id}",
              defaults: new { controller = "User", action = "Portfolio" }
            );

            routes.MapRoute(
              name: "recruitmentPage",
              url: "recruitment/{id}",
              defaults: new { controller = "Partner", action = "JobDetail" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
