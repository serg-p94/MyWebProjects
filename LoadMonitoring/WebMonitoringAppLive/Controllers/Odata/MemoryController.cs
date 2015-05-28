using System;
using System.Linq;
using System.Web.OData;
using BusinessLogic.MonitoringMemory;
using BusinessLogic.MonitoringMemory.DAL_Interfaces;

namespace WebMonitoringAppLive.Controllers.Odata
{
    public class MemoryController : ODataController
    {
        private readonly IMemoryStatisticsLoader _msl;

        public MemoryController()
        {
            _msl = Bootstrapper.Bootstrapper.GetMemoryStatisticsLoader();
        }

        [EnableQuery]
        public IQueryable<MemoryState> Get()
        {
            return _msl.LoadMemoryStatistics(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
        }
    }
}