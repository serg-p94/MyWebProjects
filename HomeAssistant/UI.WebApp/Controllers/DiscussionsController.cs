using System.Web.Mvc;
using System.Linq;
using UI.WebApp.Helpers;
using Bootstrapper;

namespace UI.WebApp.Controllers
{
    [Authorize(Roles = "Read Forum")]
    public class DiscussionsController : BaseController
    {
        public ActionResult ShowAll()
        {
            ViewBag.MenuItem = MenuItem.Discussions;
            var dm = Loader.GetDiscussionManager();
            return View(dm.GetDiscussions());
        }

        public ActionResult Show(int? id)
        {
            ViewBag.MenuItem = MenuItem.Discussions;
            if (!id.HasValue)
            {
                return RedirectToAction("ShowAll");
            }
            var dm = Loader.GetDiscussionManager();
            var discussion = dm.GetDiscussions().SingleOrDefault(d => d.Id == id);
            return View(discussion);
        }
    }
}