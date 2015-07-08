using System.Collections.Generic;
namespace BL.Users
{
    public interface IPermissionManager
    {
        HashSet<Permission> GetPermissions();
        bool PermissionExists(string name);
        Permission this[string name] { get; }
    }
}
