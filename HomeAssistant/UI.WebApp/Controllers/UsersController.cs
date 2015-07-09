﻿using BL.Users;
using Bootstrapper;
using System.Web.Mvc;
using System.Web.Security;
using UI.WebApp.Helpers;
using System.Linq;

namespace UI.WebApp.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Registration(string login, string password)
        {
            ViewBag.MenuItem = MenuItem.User;
            if (login == null && password == null)
            {
                return View(new User());
            }
            else
            {
                var um = Bootstrapper.Loader.GetUserManager();
                return RedirectToAction("RegistrationResult", new { result = um.Register(new User { Login = login, Password = password }) });
            }
        }

        public ActionResult RegistrationResult(UserRegistrationResult result)
        {
            ViewBag.MenuItem = MenuItem.User;
            return View(result);
        }

        public ActionResult LogIn(string login, string password)
        {
            ViewBag.MenuItem = MenuItem.User;
            if (login == null && password == null)
            {
                return View(new User());
            }
            else
            {
                var um = Bootstrapper.Loader.GetUserManager();
                var result = um.Validate(login, password);
                if (result == UserValidationResult.Success)
                {
                    var user = um.Users.Single(u => u.Login == login);
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                    Session["UserName"] = user.Login;
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ShowAll()
        {
            ViewBag.MenuItem = MenuItem.User;
            var um = Loader.GetUserManager();
            return View(um.Users);
        }

        public ActionResult Details(int? id)
        {
            ViewBag.MenuItem = MenuItem.User;
            var um = Loader.GetUserManager();
            return View(um.Users.SingleOrDefault(u => u.Id == id));
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}