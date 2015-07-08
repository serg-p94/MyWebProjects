using BL.Users;
using System.Web.Mvc;

namespace UI.WebApp.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Register(string login, string password)
        {
            if (login == null && password == null)
            {
                return View();
            }
            else
            {
                var um = Bootstrapper.Bootstrapper.GetUserManager();
                return RedirectToAction("RegistrationResult", new { result = um.Register(new User { Login = login, Password = password }) });
            }
        }

        public ActionResult RegistrationResult(UserRegistrationResult result)
        {
            return View(result);
        }
    }
}