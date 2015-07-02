using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcCalculator.Models;
using MvcCalculator.Helper;
using System.Web.Security;

namespace MvcCalculator.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Registration(string name, string password, string email, string birthDate)
        {
            ViewBag.MainMenu = MainMenuItems.User;
            DateTime dtBirthDate;
            RegistrationViewModel model;
            if (birthDate != null &&
                DateTime.TryParse(birthDate, new CultureInfo("ru-RU", false), DateTimeStyles.None, out dtBirthDate))
            {
                model = new RegistrationViewModel(
                    new User(name, password, email,
                        DateTime.Parse(birthDate, new CultureInfo("ru-RU", false))));
            }
            else
            {
                model = new RegistrationViewModel(new User(name, password, email, null));
            }
            if (name != null && password != null && email != null
                && model.User.Information.BirthDate != null)
            {
                model.Result = new UserManager().Register(model.User);
                var msg = new Message();
                msg.Header = "Registration";
                if (model.Result == UserRegistrationResult.Success)
                {
                    LogIn(name, password, null);
                    msg.Type = Message.MessageType.Success;
                    msg.Body = "User " + name + " was registered!";
                    return RedirectToAction("ShowMessage", "Home", msg);
                }
            }
            return View(model);
        }

        public ActionResult LogIn(string name, string password, string remember)
        {
            ViewBag.MainMenu = MainMenuItems.User;
            var model = new LogInViewModel(name, password);
            if (name != null && password != null)
            {
                var um = new UserManager();
                model.Result = um.ValidateUser(name, password);
                if (model.Result == UserValidationResult.Success)
                {
                    FormsAuthentication.SetAuthCookie(name, remember == "on");
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult IsNameAvailable(string name)
        {
            var result = new {IsNameAvailable = !new UserManager().UserExists(name)};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DeleteCurrentUser()
        {
            new UserManager().DeleteUser(User.Identity.Name);
            return LogOut();
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult UserInfo(string name)
        {
            var um = new UserManager();
            if (name != null)
            {
                if (HttpContext.User.Identity.Name == name)
                {
                    ViewBag.MainMenu = MainMenuItems.User;
                }
                return View(um.GetInfo(um.GetUserId(name)));
            }
            ViewBag.MainMenu = MainMenuItems.User;
            return View(um.GetInfo(um.GetUserId(HttpContext.User.Identity.Name)));
        }

        [Authorize]
        public ActionResult ShowAll()
        {
            ViewBag.MainMenu = MainMenuItems.User;
            return View(model: new UserManager().GetAllNames());
        }

        public ActionResult PasswordRecovery(string email)
        {
            ViewBag.MainMenu = MainMenuItems.User;
            if (email == null)
                return View();
            var msg = new Message();
            msg.Header = "Password Recovery";
            var res = new UserManager().SendLoginPwd(email);
            if (res.Type == Message.MessageType.Success)
            {
                msg.Type = Message.MessageType.Success;
                msg.Body = "Check your email for login and password.";
            }
            else
            {
                msg.Type = Message.MessageType.Error;
                msg.Body = "Email may be incorrect.";
            }
            return RedirectToAction("ShowMessage", "Home", msg);
        }
    }
}
