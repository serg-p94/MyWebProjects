using System;
using System.Linq;

namespace BusinessLogic.MonitoringMemory.DAL_Interfaces
{
    public interface IMemoryStatisticsLoader
    {
        IQueryable<MemoryState> LoadMemoryStatistics(DateTime startDate, DateTime endDate);
    }
}
