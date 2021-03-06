﻿using System.Collections.Generic;
using System.Security.Principal;
using BL.Users;
using Bootstrapper;

namespace UI.WebApp.Helpers.Users
{
    public class CustomPrincipal : ICustomPrincipal
    {
        protected CustomPrincipal()
        {
            Permissions = new HashSet<Permission>(new PermissionEqualityComparer());
        }

        public CustomPrincipal(string id) : this()
        {
            Identity = new GenericIdentity(id);
        }

        public CustomPrincipal(IIdentity identity) : this()
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            var pm = Loader.GetPermissionManager();
            return Permissions.Contains(pm[role]);
        }

        public IIdentity Identity { get; private set; }

        public int Id { get; set; }
        public string Login { get; set; }
        public HashSet<Permission> Permissions { get; protected set; }

        public bool IsMale { get; set; }
        public string Avatar { get; set; }
    }
}