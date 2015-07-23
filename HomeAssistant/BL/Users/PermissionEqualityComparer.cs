using System.Collections.Generic;

namespace BL.Users
{
    public class PermissionEqualityComparer : IEqualityComparer<Permission>
    {
        public bool Equals(Permission x, Permission y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Permission obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
