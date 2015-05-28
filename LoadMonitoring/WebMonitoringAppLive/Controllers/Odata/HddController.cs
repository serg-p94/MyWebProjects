using System;
using System.Linq;
using System.Web.OData;
using BusinessLogic.MonitoringHdd;
using BusinessLogic.MonitoringHdd.DAL_Interfaces;

namespace WebMonitoringAppLive.Controllers.Odata
{
    public class HddController : ODataController
    {
        private readonly IHddStatisticsLoader _loader;

        public HddController()
        {
            _loader = Bootstrapper.Bootstrapper.GetHddStatisticsLoader();
        }

        [EnableQuery]
        public IQueryable<HddState> Get()
        {
            return _loader.LoadHddStatistics(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
        }
    }
}