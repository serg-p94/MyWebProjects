using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebMonitoringAppLive
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Resources", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(name: "ODataRoute", url: "odata/");
        }
    }
}
