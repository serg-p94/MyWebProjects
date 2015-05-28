
using System;

namespace BusinessLogic.MonitoringHdd
{
    public class HddState
    {
        public HddState()
        {
            Date = DateTime.Now;
        }

        public int Id { get; set; }
        public virtual Drive Drive { get; set; }
        public DateTime Date { get; set; }
        public long AvailableSpace { get; set; }
    }
}
