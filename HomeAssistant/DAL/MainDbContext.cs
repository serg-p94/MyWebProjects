using System.Collections.Generic;
using System.Data.Entity;
using BL.Discussions;
using BL.Users;

namespace DAL
{
    public class MainDbContext : DbContext
    {
        private static readonly Dictionary<string, MainDbContext> Instances = new Dictionary<string, MainDbContext>();

        protected MainDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<MainDbContext>());
        }

        public static MainDbContext GetInstance(string nameOrConnectionString)
        {
            lock (Instances)
            {
                if (Instances.ContainsKey(nameOrConnectionString))
                {
                    return Instances[nameOrConnectionString];
                }
                return Instances[nameOrConnectionString] = new MainDbContext(nameOrConnectionString);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
    }
}