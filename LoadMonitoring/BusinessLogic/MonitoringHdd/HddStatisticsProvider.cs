using System;
using System.Collections.Generic;
using BusinessLogic.MonitoringHdd.DAL_Interfaces;

namespace BusinessLogic.MonitoringHdd
{
    public class HddStatisticsProvider
    {
        private readonly IHddStatisticsLoader _hddStatisticsLoader;

        public HddStatisticsProvider(IHddStatisticsLoader hddStatisticsLoader)
        {
            _hddStatisticsLoader = hddStatisticsLoader;
        }

        public IEnumerable<HddState> GetHddStatistics(DateTime startDate, DateTime endDate)
        {
            return _hddStatisticsLoader.LoadHddStatistics(startDate, endDate);
        }

        public void PrintAll()
        {
            var query = _hddStatisticsLoader.LoadHddStatistics(DateTime.MinValue, DateTime.MaxValue);
            foreach (var state in query)
            {
                Console.WriteLine("{0}: [{1}] - {2}: {3}", state.Id, state.Date, state.Drive.Name,
                    state.AvailableSpace);
            }
        }
    }
}
