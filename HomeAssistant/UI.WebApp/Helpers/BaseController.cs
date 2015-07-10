using System.Web.Mvc;

namespace UI.WebApp.Helpers
{
    public class BaseController : Controller
    {
        public virtual new CustomPrincipal User
        {
            get { return _user ?? (_user = base.User as CustomPrincipal); } 
            
        } private CustomPrincipal _user;
    }
}
