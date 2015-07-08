using BL.Users;
using Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var um = UsersBootstrapper.GetUserManager();
            var pm = UsersBootstrapper.GetPermissionManager();

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
        }
    }
}