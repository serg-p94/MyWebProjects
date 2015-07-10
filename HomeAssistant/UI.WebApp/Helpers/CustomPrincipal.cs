using System;
using System.Security.Principal;

namespace UI.WebApp.Helpers
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal(string id)
        {
            Identity = new GenericIdentity(id);
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity { get; private set; }

        public int Id { get; set; }
        public string Login { get; set; }
    }
}