using System.Collections.Generic;
using System.Text;

namespace BL.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual HashSet<Permission> Permissions { get; protected set; }

        public User()
        {
            Permissions = new HashSet<Permission>();
        }

        public bool HasPermission(Permission permission)
        {
            return Permissions.Contains(permission);
        }

        public void AddPermission(Permission permission)
        {
            Permissions.Add(permission);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var p in Permissions)
            {
                sb.Append(p + " ");
            }
            return string.Format("{0}: {1} - {2}   Permissions: {3}", Id, Login, Password, sb.ToString());
        }
    }
}
