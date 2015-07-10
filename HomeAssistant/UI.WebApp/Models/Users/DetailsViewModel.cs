using System.Collections.Generic;
using BL.Users;

namespace UI.WebApp.Models.Users
{
    public class DetailsViewModel
    {
        public User User { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }
    }
}