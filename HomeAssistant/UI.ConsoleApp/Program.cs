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
            Console.WriteLine("Exists: " + um.Exists("Admin"));
            Console.WriteLine("GetUser: " + um.GetUser("Admin"));
            Console.WriteLine("Validate: " + um.Validate("Admin", "Admin"));
            um.Register(new User { Login = "Admin", Password = "Admin" });
        }
    }
}