using System.Web.Mvc;
using UI.WebApp.Helpers;

namespace UI.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MenuItem = MenuItem.Home;
            return View();
        }
    }
}