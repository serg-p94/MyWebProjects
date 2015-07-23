using System;
using System.Data.Entity;
using System.IO;
using BL.Discussions;
using BL.Users;

namespace DAL
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<MainDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
    }
}