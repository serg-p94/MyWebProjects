using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Bootstrapper;
using UI.WebApp.Helpers;
using UI.WebApp.Helpers.Users;

namespace UI.WebApp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                HttpContext.Current.User = new CustomPrincipal(new GenericIdentity(string.Empty));
                return;
            }
            try
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var userId = int.Parse(authTicket.Name);
                var um = Loader.GetUserManager();
                var user = um.Users.Single(u => u.Id == userId);
                var principal = new CustomPrincipal(authTicket.Name)
                {
                    Id = user.Id,
                    Login = user.Login,
                    IsMale = user.IsMale,
                    Avatar = user.Avatar
                };
                principal.Permissions.UnionWith(user.Permissions);

                HttpContext.Current.User = principal;
            }
            catch
            {
                HttpContext.Current.User = new CustomPrincipal(new GenericIdentity(string.Empty));
            }

        }
    }
}