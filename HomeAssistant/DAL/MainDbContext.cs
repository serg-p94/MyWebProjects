using BL.Discussions;
using BL.Users;
using System.Data.Entity;
using System.Collections.Generic;

namespace DAL
{
    public class MainDbContext : DbContext
    {
        private static Dictionary<string, MainDbContext> _instances = new Dictionary<string, MainDbContext>();

        protected MainDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            //Database.SetInitializer<MainDbContext>(new DropCreateDatabaseAlways<MainDbContext>());
        }

        public static MainDbContext GetInstance(string nameOrConnectionString)
        {
            lock (_instances)
            {
                if (_instances.ContainsKey(nameOrConnectionString))
                {
                    return _instances[nameOrConnectionString];
                }
                return _instances[nameOrConnectionString] = new MainDbContext(nameOrConnectionString);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
    }
}