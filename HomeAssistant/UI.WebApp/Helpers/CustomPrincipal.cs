using System.Collections.Generic;
using System.Security.Principal;
using BL.Users;
using Bootstrapper;

namespace UI.WebApp.Helpers
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal(string id)
        {
            Identity = new GenericIdentity(id);
            Permissions = new HashSet<Permission>();
        }

        public bool IsInRole(string role)
        {
            var pm = Loader.GetPermissionManager();
            var permission = pm[role];
            return permission != null && Permissions.Contains(permission);
        }

        public IIdentity Identity { get; private set; }

        public int Id { get; set; }
        public string Login { get; set; }
        public HashSet<Permission> Permissions { get; protected set; }
    }
}