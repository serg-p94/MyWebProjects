using BusinessLogic.MonitoringHdd;
using BusinessLogic.MonitoringHdd.DAL_Interfaces;
using BusinessLogic.MonitoringMemory;
using BusinessLogic.MonitoringMemory.DAL_Interfaces;
using DataLayer;
using Microsoft.Practices.Unity;

namespace Bootstrapper
{
    public static class Bootstrapper
    {
        private static readonly UnityContainer Container;
        static Bootstrapper()
        {
            Container = new UnityContainer();

            Container.RegisterType<IMemoryMonitor, MemoryMonitor>();
            Container.RegisterType<IHddMonitor, HddMonitor>();

            Container.RegisterType<IMemoryStateWriter, ResourcesDbManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMemoryStatisticsLoader, ResourcesDbManager>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IHddStateWriter, ResourcesDbManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IHddStatisticsLoader, ResourcesDbManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IHddDriveLoader, ResourcesDbManager>(new ContainerControlledLifetimeManager());
        }

        public static IMemoryMonitor GetMemoryMonitor()
        {
            return Container.Resolve<IMemoryMonitor>();
        }

        public static IMemoryStatisticsLoader GetMemoryStatisticsLoader()
        {
            return Container.Resolve<IMemoryStatisticsLoader>();
        }

        public static IHddMonitor GetHddMonitor()
        {
            return Container.Resolve<IHddMonitor>();
        }

        public static IHddStatisticsLoader GetHddStatisticsLoader()
        {
            return Container.Resolve<IHddStatisticsLoader>();
        }

        public static IHddDriveLoader GetHddDriveLoader()
        {
            return Container.Resolve<IHddDriveLoader>();
        }
    }
}
