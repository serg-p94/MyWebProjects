using System.Web.Mvc;
using UI.WebApp.Helpers;

namespace UI.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.MenuItem = MenuItem.Home;
            return View();
        }
    }
}