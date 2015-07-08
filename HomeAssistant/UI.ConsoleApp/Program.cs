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
            var um = Bootstrapper.Bootstrapper.GetUserManager();
            var pm = Bootstrapper.Bootstrapper.GetPermissionManager();

            um.Register(new User { Login = "Admin", Password = "Admin" });
            um.Register(new User { Login = "Sergei", Password = "111" });
            
            Console.WriteLine("Exists: " + um.Exists("Admin"));
            Console.WriteLine("GetUser: " + um["Admin"]);
            Console.WriteLine("Validate: " + um.Validate("Admin", "Admin"));

            um["Admin"].AddPermission(pm["Read Forum"]);
            um["Sergei"].AddPermission(pm["Read Forum"]);
            um["Sergei"].AddPermission(pm["Write Forum"]);
            um.Update();

            Console.WriteLine("Exists: " + um.Exists("Admin"));
            Console.WriteLine("GetUser: " + um["Admin"]);
            Console.WriteLine("Validate: " + um.Validate("Admin", "Admin"));


            var dm = Bootstrapper.Bootstrapper.GetDiscussionManager();
            dm.CreateDiscussion("Test disc");
            dm.GetDiscussions().FirstOrDefault().Messages.Add(new Message
            {
                Author = Bootstrapper.Bootstrapper.GetUserManager()["Admin"],
                Body = "Test msg",
                Date = DateTime.Now
            });
            dm.Update();
        }
    }
}