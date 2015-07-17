using System.Collections.Generic;

namespace BL.Users
{
    public interface IPermissionManager
    {
        HashSet<Permission> Permissions { get; }
        bool PermissionExists(string name);
        Permission this[string name] { get; }
    }
}
