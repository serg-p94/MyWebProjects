using BL.Users;
using System.Data.Entity;

namespace DAL
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
