using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BL.Users;

namespace DAL
{
    public class UserManager : IUserManager, IPermissionManager
    {
        private readonly MainDbContext _context;

        public static readonly string NameOrConnectionString =
            ConfigurationManager.ConnectionStrings["MainDbContext"].ConnectionString;

        public UserManager()
        {
            _context = new MainDbContext(NameOrConnectionString);
        }

        public UserValidationResult Validate(string login, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Login == login);
            if (user == null)
            {
                return UserValidationResult.NotFound;
            }
            return user.Password == password ? UserValidationResult.Success : UserValidationResult.WrongPassword;
        }

        public bool Exists(string login)
        {
            return _context.Users.Count(u => u.Login == login) > 0;
        }

        IQueryable<User> IUserReader.Users
        {
            get { return _context.Users; }
        }

        User IUserReader.this[string login]
        {
            get { return _context.Users.SingleOrDefault(u => u.Login == login); }
        }

        public UserRegistrationResult Register(User user)
        {
            if (Exists(user.Login))
            {
                return UserRegistrationResult.AlreadyExists;
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return UserRegistrationResult.Success;
        }

        public void Remove(string login)
        {
            var user = _context.Users.SingleOrDefault(u => u.Login == login);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public void Update()
        {
            _context.SaveChanges();
        }

        public HashSet<Permission> Permissions
        {
            get { return new HashSet<Permission>(_context.Permissions); }
        }

        public bool PermissionExists(string name)
        {
            return _context.Permissions.Any(p => p.Name == name);
        }

        Permission IPermissionManager.this[string name]
        {
            get
            {
                if (!PermissionExists(name))
                {
                    _context.Permissions.Add(new Permission { Name = name });
                    _context.SaveChanges();
                }
                return _context.Permissions.Single(p => p.Name == name);
            }
        }
    }
}
