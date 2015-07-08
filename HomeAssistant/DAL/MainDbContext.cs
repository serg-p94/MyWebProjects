using BL.Users;
using System;
using System.Data.Entity;

namespace DAL
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<MainDbContext>(new DropCreateDatabaseAlways<MainDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
    }
}
