using System.Data.Entity;
using BusinessLogic.MonitoringHdd;
using BusinessLogic.MonitoringMemory;

namespace DataLayer
{
    internal class ResourcesDbContext : DbContext
    {
        public DbSet<MemoryState> MemoryStatistics { get; set; }
        public DbSet<HddState> HddStatistics { get; set; }
        public DbSet<Drive> Drives { get; set; }
    }
}
