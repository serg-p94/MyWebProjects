using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BL.Users;
using Bootstrapper;
using UI.WebApp.Helpers;
using UI.WebApp.Models.Users;

namespace UI.WebApp.Controllers
{
    public class UsersController : BaseController
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
                var um = Loader.GetUserManager();
                return RedirectToAction("RegistrationResult", new { result = um.Register(new User { Login = login, Password = password }) });
            }
        }

        public ActionResult RegistrationResult(UserRegistrationResult result)
        {
            ViewBag.MenuItem = MenuItem.User;
            return View(result);
        }

        public ActionResult LogIn(string login, string password, string remember)
        {
            ViewBag.MenuItem = MenuItem.User;
            if (login == null && password == null)
            {
                return View(new User());
            }
            else
            {
                var um = Loader.GetUserManager();
                var result = um.Validate(login, password);
                if (result == UserValidationResult.Success)
                {
                    var user = um.Users.Single(u => u.Login == login);
                    var authTicket = new FormsAuthenticationTicket(version: 1, name: user.Id.ToString(),
                        issueDate: DateTime.Now, expiration: DateTime.MaxValue,
                        isPersistent: remember == "on", userData: user.Id.ToString());
                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    if (authTicket.IsPersistent)
                    {
                        authCookie.Expires = authTicket.Expiration;
                    }
                    Response.Cookies.Add(authCookie);
                }
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = UserRole.BrowseUsers)]
        public ActionResult ShowAll()
        {
            ViewBag.MenuItem = MenuItem.User;
            var um = Loader.GetUserManager();
            return View(um.Users);
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue || id != User.Id && !User.IsInRole(UserRole.BrowseUsers))
            {
                return RedirectToAction("AccessDenied", new {ReturnUrl = Request.Url.AbsolutePath});
            }
            ViewBag.MenuItem = MenuItem.User;
            var um = Loader.GetUserManager();
            var pm = Loader.GetPermissionManager();
            var viewModel = new DetailsViewModel
            {
                User = um.Users.SingleOrDefault(u => u.Id == id),
                Permissions = pm.Permissions
            };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccessDenied(string returnUrl)
        {
            ViewBag.MenuItem = MenuItem.User;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogIn");
            }
            var url = string.Format("{0}{1}{2}:{3}{4}", Request.Url.Scheme, Uri.SchemeDelimiter, Request.Url.Host,
                Request.Url.Port, returnUrl);
            return View(new AccessDeniedViewModel {Url = url});
        }

        [Authorize(Roles = UserRole.ChangePermissions)]
        public JsonResult SetUserPermission(int userId, int permissionId, bool value)
        {
            var um = Loader.GetUserManager();
            var pm = Loader.GetPermissionManager();
            var user = um.Users.Single(u => u.Id == userId);
            var permission = pm.Permissions.Single(p => p.Id == permissionId);
            if (user.HasPermission(permission) == value)
            {
                return new JsonResult {Data = new {result = "sucess"}};
            }
            if (value)
            {
                user.AddPermission(permission);
            }
            else
            {
                user.RemovePermission(permission);
            }
            um.Update();
            return new JsonResult { Data = new { result = "sucess" }};
        }
    }
}