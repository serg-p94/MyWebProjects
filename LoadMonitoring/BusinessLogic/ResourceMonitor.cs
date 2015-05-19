using System.Diagnostics;
using System.Threading;

namespace BusinessLogic
{
    public abstract class ResourceMonitor : IResourceMonitor
    {
        private readonly Thread _monitoringThread;
        private bool _threadActive;

        private readonly object _locker = new object();

        protected PerformanceCounter Pc;

        public int MillisecondsTimeout { get; set; }

        protected ResourceMonitor()
        {
            MillisecondsTimeout = 1000;
            _monitoringThread = new Thread(MonitoringThreadFunction);
            _monitoringThread.Name = "Monitoring Thread";
        }

        public void Start()
        {
            lock (_locker)
            {
                _threadActive = true;
                _monitoringThread.Start();
            }
        }

        public void Stop()
        {
            lock (_locker)
            {
                _threadActive = false;
            }
            _monitoringThread.Join();
        }

        private void MonitoringThreadFunction()
        {
            while (true)
            {
                lock (_locker)
                {
                    if (!_threadActive)
                        break;
                }

                DumpResourceInfo();

                Thread.Sleep(MillisecondsTimeout);
            }
        }

        //Gets resource usage info and writes it somewhere
        protected abstract void DumpResourceInfo();
    }
}