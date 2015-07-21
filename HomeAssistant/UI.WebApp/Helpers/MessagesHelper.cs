using System.Web;

namespace UI.WebApp.Helpers
{
    public static class MessagesHelper
    {
        public static HtmlString SuccessMessage(string text)
        {
            return new HtmlString(string.Format("<h3 class=\"text-center alert alert-success\">{0}</h3>", text));
        }

        public static HtmlString InfoMessage(string text)
        {
            return new HtmlString(string.Format("<h3 class=\"text-center alert alert-info\">{0}</h3>", text));
        }

        public static HtmlString WarningMessage(string text)
        {
            return new HtmlString(string.Format("<h3 class=\"text-center alert alert-warning\">{0}</h3>", text));
        }

        public static HtmlString ErrorMessage(string text)
        {
            return new HtmlString(string.Format("<h3 class=\"text-center alert alert-danger\">{0}</h3>", text));
        }
    }
}
