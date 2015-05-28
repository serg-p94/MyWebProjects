using System;
using System.Linq;

namespace BusinessLogic.MonitoringHdd.DAL_Interfaces
{
    public interface IHddStatisticsLoader
    {
        IQueryable<HddState> LoadHddStatistics(DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
