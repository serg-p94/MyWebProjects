using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcCalculator.Helper;

namespace MvcCalculator.Models
{
    public class LogInViewModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public UserValidationResult Result { get; set; }

        public LogInViewModel(string name, string password)
        {
            Name = name;
            Password = password;
            Result = UserValidationResult.No;
        }
    }
}