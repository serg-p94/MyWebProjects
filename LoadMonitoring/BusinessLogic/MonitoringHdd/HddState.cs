
using System;

namespace BusinessLogic.MonitoringHdd
{
    public class HddState
    {
        public HddState()
        {
            Date = DateTimeOffset.Now;
        }

        public int Id { get; set; }
        public virtual Drive Drive { get; set; }
        public DateTimeOffset Date { get; set; }
        public long AvailableSpace { get; set; }
    }
}
