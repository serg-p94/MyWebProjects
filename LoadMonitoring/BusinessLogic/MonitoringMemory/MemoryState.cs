using System;

namespace BusinessLogic.MonitoringMemory
{
    public class MemoryState
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public int AvailableMemoryMb { get; set; }
    }
}
