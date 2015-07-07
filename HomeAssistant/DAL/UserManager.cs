﻿using BL.Users;
using System.Configuration;
using System.Linq;

namespace DAL
{
    public class UserManager : IUserManager
    {
        private readonly MainDbContext _context;
        public static readonly string NameOrConnectionString = ConfigurationManager.ConnectionStrings["MainDbContext"].ConnectionString;

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

        public User GetUser(string login)
        {
            return _context.Users.SingleOrDefault(u => u.Login == login);
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
    }
}
