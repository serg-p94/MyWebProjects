using BL.Users;
using System.Web.Mvc;
using System.Web.Security;

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

        public ActionResult LogIn(string login, string password)
        {
            if (login == null && password == null)
            {
                return View();
            }
            else
            {
                var um = Bootstrapper.Bootstrapper.GetUserManager();
                var result = um.Validate(login, password);
                if (result == UserValidationResult.Success)
                {
                    FormsAuthentication.SetAuthCookie(login, false);
                }
                return RedirectToAction("LogInResult", new { result = result });
            }
        }

        public ActionResult LogInResult(UserValidationResult result)
        {
            return View(result);
        }
    }
}