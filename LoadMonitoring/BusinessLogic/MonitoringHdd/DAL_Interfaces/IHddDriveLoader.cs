using System.Linq;

namespace BusinessLogic.MonitoringHdd.DAL_Interfaces
{
    public interface IHddDriveLoader
    {
        Drive GetDrive(string name);
        IQueryable<Drive> GetAllDrives();
    }
}
