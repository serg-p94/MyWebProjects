using System;
using System.IO;
using BusinessLogic.MonitoringHdd.DAL_Interfaces;

namespace BusinessLogic.MonitoringHdd
{
    public class HddMonitor : ResourceMonitor, IHddMonitor
    {
        private readonly IHddStateWriter _hddStateWriter;
        private readonly IHddDriveLoader _hddDriveLoader;

        public HddMonitor(IHddStateWriter hddStateWriter, IHddDriveLoader hddDriveLoader)
        {
            _hddStateWriter = hddStateWriter;
            _hddDriveLoader = hddDriveLoader;
        }

        protected override void DumpResourceInfo()
        {
            foreach (var di in DriveInfo.GetDrives())
            {
                if (di.IsReady && (di.DriveType == DriveType.Fixed || di.DriveType == DriveType.Removable))
                {
                    var drive = _hddDriveLoader.GetDrive(di.Name);
                    if (drive == null)
                    {
                        drive = new Drive() {Name = di.Name};
                    }
                    var state = new HddState()
                    {
                        Drive = drive,
                        Date = DateTime.Now,
                        AvailableSpace = di.AvailableFreeSpace
                    };
                    _hddStateWriter.WriteHddState(state);
                }
            }
        }
    }
}
