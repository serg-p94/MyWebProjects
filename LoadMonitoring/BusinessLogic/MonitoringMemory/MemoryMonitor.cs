using System;
using System.Diagnostics;
using BusinessLogic.MonitoringHdd;
using BusinessLogic.MonitoringMemory.DAL_Interfaces;

namespace BusinessLogic.MonitoringMemory
{
    public class MemoryMonitor : ResourceMonitor, IMemoryMonitor
    {
        private readonly IMemoryStateWriter _mid;

        public MemoryMonitor(IMemoryStateWriter mid)
        {
            _mid = mid;
            Pc = new PerformanceCounter("Memory", "Available MBytes");
        }

        protected override void DumpResourceInfo()
        {
            var memState = new MemoryState();
            memState.Date = DateTimeOffset.Now;
            memState.AvailableMemoryMb = (int) Pc.NextValue();
            _mid.WriteMemoryState(memState);
        }
    }
}