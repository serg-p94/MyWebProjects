using BL.Discussions;
using BL.Users;
using Bootstrapper;
using System;
using System.Linq;

namespace UI.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var um = Bootstrapper.Loader.GetUserManager();
            var pm = Bootstrapper.Loader.GetPermissionManager();

            um.Register(new User { Login = "Admin", Password = "Admin" });

            um["Admin"].AddPermission(pm["Read Forum"]);
            um["Admin"].AddPermission(pm["Write Forum"]);
            um.Update();

            var dm = Bootstrapper.Loader.GetDiscussionManager();
            dm.CreateDiscussion("First discussion");
            dm.CreateDiscussion("Second discussion");
            var disc = dm.GetDiscussions().First();
            var admin = um["Admin"];
            disc.Messages.Add(new Message
            {
                Author = admin,
                Body = "Test msg",
                Date = DateTime.Now
            });
            dm.Update();
        }
    }
}