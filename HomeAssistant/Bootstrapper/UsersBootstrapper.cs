using BL.Users;
using DAL;
using Microsoft.Practices.Unity;

namespace Bootstrapper
{
    public class UsersBootstrapper
    {
        private static UnityContainer _container = new UnityContainer();

        static UsersBootstrapper()
        {
            _container.RegisterType<IUserManager, UserManager>(new PerThreadLifetimeManager());
            _container.RegisterType<IPermissionManager, UserManager>(new PerThreadLifetimeManager());
        }

        public static IUserManager GetUserManager()
        {
            return _container.Resolve<IUserManager>();
        }

        public static IPermissionManager GetPermissionManager()
        {
            return _container.Resolve<IPermissionManager>();
        }
    }
}
