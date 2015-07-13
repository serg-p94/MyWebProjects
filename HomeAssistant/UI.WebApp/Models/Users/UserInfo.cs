using System.Collections.Generic;
using BL.Users;

namespace UI.WebApp.Models.Users
{
    public class UserInfo
    {
        public UserInfo()
        {
            PermissionIds = new List<int>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}