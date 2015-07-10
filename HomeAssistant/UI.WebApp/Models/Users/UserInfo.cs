using System.Collections.Generic;
using BL.Users;

namespace UI.WebApp.Models.Users
{
    public class UserInfo
    {
        public UserInfo()
        {
            Permissions = new HashSet<Permission>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public HashSet<Permission> Permissions { get; set; }
    }
}