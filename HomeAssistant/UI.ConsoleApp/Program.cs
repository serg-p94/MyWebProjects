using System;
using System.Linq;
using BL.Discussions;
using BL.Users;
using Bootstrapper;

namespace UI.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var um = Loader.GetUserManager();
            var pm = Loader.GetPermissionManager();

            um.Register(new User { Login = "Admin", Password = "Admin" });

            um["Admin"].AddPermission(pm["Read Forum"]);
            um["Admin"].AddPermission(pm["Write Forum"]);
            um.Update();

            var dm = Loader.GetDiscussionManager();
            dm.CreateDiscussion("First discussion");
            dm.CreateDiscussion("Second discussion");
            var disc = dm.Discussions.First();
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