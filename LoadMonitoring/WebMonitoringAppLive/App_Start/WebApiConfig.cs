using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using BusinessLogic.MonitoringHdd;
using BusinessLogic.MonitoringMemory;

namespace WebMonitoringAppLive
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<MemoryState>("Memory");
            builder.EntitySet<Drive>("Drive");
            builder.EntitySet<HddState>("Hdd");
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());
        }
    }
}
