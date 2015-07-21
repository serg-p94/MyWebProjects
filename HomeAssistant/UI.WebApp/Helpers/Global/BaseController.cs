using System.Web.Mvc;
using UI.WebApp.Helpers.Users;

namespace UI.WebApp.Helpers.Global
{
    public class BaseController : Controller
    {
        public virtual new CustomPrincipal User
        {
            get { return _user ?? (_user = base.User as CustomPrincipal); } 
            
        } private CustomPrincipal _user;
    }
}
