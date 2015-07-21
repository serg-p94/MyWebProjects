using BL.Discussions;
using BL.Users;
using DAL;
using Microsoft.Practices.Unity;

namespace Bootstrapper
{
    public class Loader
    {
        private static readonly UnityContainer Container = new UnityContainer();

        static Loader()
        {
            Container.RegisterType<IUserManager, UserManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IPermissionManager, UserManager>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IDiscussionManager, DiscussionManager>();

            Container.RegisterType<IMailSender, MailSender>();
        }

        public static IUserManager GetUserManager()
        {
            return Container.Resolve<IUserManager>();
        }

        public static IPermissionManager GetPermissionManager()
        {
            return Container.Resolve<IPermissionManager>();
        }

        public static IDiscussionManager GetDiscussionManager()
        {
            return Container.Resolve<IDiscussionManager>();
        }

        public static IMailSender GetMailSender(string from, string password)
        {
            return Container.Resolve<IMailSender>(new ParameterOverride("from", from),
                new ParameterOverride("password", password));
        }
    }
}
