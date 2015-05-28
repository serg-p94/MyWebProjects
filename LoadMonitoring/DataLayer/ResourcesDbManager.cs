using System;
using System.Linq;
using BusinessLogic.MonitoringHdd;
using BusinessLogic.MonitoringHdd.DAL_Interfaces;
using BusinessLogic.MonitoringMemory;
using BusinessLogic.MonitoringMemory.DAL_Interfaces;

namespace DataLayer
{
    public class ResourcesDbManager : IMemoryStatisticsLoader, IMemoryStateWriter, IHddStateWriter, IHddStatisticsLoader,
        IHddDriveLoader
    {
        private readonly ResourcesDbContext _context;

        public ResourcesDbManager()
        {
            Console.WriteLine("ResourcesDbManager()");
            //Database.SetInitializer(new ResourcesDbInitializer());
            _context = new ResourcesDbContext();
        }

        public void WriteMemoryState(MemoryState memState)
        {
            lock (_context)
            {
                _context.MemoryStatistics.Add(memState);
                _context.SaveChanges();
            }
        }

        public IQueryable<MemoryState> LoadMemoryStatistics(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            lock (_context)
            {
                return _context.MemoryStatistics.Where(ms => ms.Date >= startDate && ms.Date <= endDate);
            }
        }

        public void WriteHddState(HddState hddState)
        {
            lock (_context)
            {
                _context.HddStatistics.Add(hddState);
                _context.SaveChanges();
            }
        }

        public IQueryable<HddState> LoadHddStatistics(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            lock (_context)
            {
                return _context.HddStatistics.Where(state => startDate <= state.Date && state.Date <= endDate);
            }
        }

        public Drive GetDrive(string name)
        {
            lock (_context)
            {
                var dr = _context.Drives.Where(drive => drive.Name.Equals(name));
                return dr.Any() ? dr.First() : null;
            }
        }

        public IQueryable<Drive> GetAllDrives()
        {
            lock (_context)
            {
                return _context.Drives;
            }
        }
    }
}
