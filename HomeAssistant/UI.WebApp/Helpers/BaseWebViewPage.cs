using System.Web.Mvc;

namespace UI.WebApp.Helpers
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