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
        }

        public static IUserManager GetUserManager()
        {
            return _container.Resolve<IUserManager>();
        }
    }
}
