using System.Web.Mvc;
using UI.WebApp.Helpers.Users;

namespace UI.WebApp.Helpers.Global
{
    public abstract class BaseWebViewPage : WebViewPage
    {
        private CustomPrincipal _user;

        public virtual new CustomPrincipal User
        {
            get { return _user ?? (_user = base.User as CustomPrincipal); }
        }
    }

    public abstract class BaseWebViewPage<TModel> : WebViewPage<TModel>
    {
        private CustomPrincipal _user;

        public virtual new CustomPrincipal User
        {
            get { return _user ?? (_user = base.User as CustomPrincipal); }
        }
    }
}