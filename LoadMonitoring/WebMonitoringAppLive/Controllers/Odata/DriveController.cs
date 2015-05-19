using System.Linq;
using System.Web.OData;
using BusinessLogic.MonitoringHdd;
using BusinessLogic.MonitoringHdd.DAL_Interfaces;

namespace WebMonitoringAppLive.Controllers.Odata
{
    public class DriveController : ODataController
    {
        private readonly IHddDriveLoader _loader;

        public DriveController()
        {
            _loader = Bootstrapper.Bootstrapper.GetHddDriveLoader();
        }

        [EnableQuery]
        public IQueryable<Drive> Get()
        {
            return _loader.GetAllDrives();
        }
    }
}