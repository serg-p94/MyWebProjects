using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Bootstrapper;
using UI.WebApp.Helpers;
using UI.WebApp.Models.Users;

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
                return;
            }
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var userInfo = new JavaScriptSerializer().Deserialize<UserInfo>(authTicket.UserData);
            var user = new CustomPrincipal(authTicket.Name) {Id = userInfo.Id, Login = userInfo.Login};
            var pm = Loader.GetPermissionManager();
            user.Permissions.UnionWith(userInfo.PermissionIds.Select(pi => pm.Permissions.Single(p => p.Id == pi)));

            HttpContext.Current.User = user;
        }
    }
}
