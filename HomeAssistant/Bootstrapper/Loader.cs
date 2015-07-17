using BL.Discussions;
using BL.Users;
using DAL;
using Microsoft.Practices.Unity;

namespace Bootstrapper
{
    public class Loader
    {
        private static UnityContainer _container = new UnityContainer();

        static Loader()
        {
            _container.RegisterType<IUserManager, UserManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPermissionManager, UserManager>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IDiscussionManager, DiscussionManager>();
        }

        public static IUserManager GetUserManager()
        {
            return _container.Resolve<IUserManager>();
        }

        public static IPermissionManager GetPermissionManager()
        {
            return _container.Resolve<IPermissionManager>();
        }

        public static IDiscussionManager GetDiscussionManager()
        {
            return _container.Resolve<IDiscussionManager>();
        }
    }
}
