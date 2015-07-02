using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcCalculator.Helper;

namespace MvcCalculator.Models
{
    public class RegistrationViewModel
    {
        public User User { get; set; }
        public UserRegistrationResult Result { get; set; }
        public RegistrationViewModel(User user)
        {
            this.User = user;
            Result = UserRegistrationResult.No;
        }
    }
}