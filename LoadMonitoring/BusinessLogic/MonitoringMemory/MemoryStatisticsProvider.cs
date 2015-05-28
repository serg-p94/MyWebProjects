using System;
using System.Collections.Generic;
using BusinessLogic.MonitoringMemory.DAL_Interfaces;

namespace BusinessLogic.MonitoringMemory
{
    public class MemoryStatisticsProvider
    {
        private readonly IMemoryStatisticsLoader _msl;

        public MemoryStatisticsProvider(IMemoryStatisticsLoader msl)
        {
            _msl = msl;
        }

        public IEnumerable<MemoryState> GetStatistics(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return _msl.LoadMemoryStatistics(startDate, endDate);
        }

        public void PrintAll()
        {
            foreach (var ms in _msl.LoadMemoryStatistics(DateTimeOffset.MinValue, DateTimeOffset.MaxValue))
            {
                Console.WriteLine("{0}: [{1}] - {2} Mb", ms.Id, ms.Date, ms.AvailableMemoryMb);
            }
        }
    }
}
