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
        public ActionResult Registration(User user)
        {
            ViewBag.MenuItem = MenuItem.User;
            if (user.Login == null || user.Password == null)
            {
                return View(new UserRegistrationResultViewModel());
            }

            try
            {
                user.Permissions.Add(Loader.GetPermissionManager()[UserRole.ChangePermissions]);

                var um = Loader.GetUserManager();
                var result = um.Register(user);
                if (result == UserRegistrationResult.Success)
                {
                    var avatar = Request.Files["avatar"];
                    new AvatarsHelper(um).ChangeAvatar(user, avatar);
                }
                return View(new UserRegistrationResultViewModel {HasResult = true, Result = result});
            }
            catch
            {
                return
                    View(new UserRegistrationResultViewModel {HasResult = true, Result = UserRegistrationResult.Error});
            }
        }

        public ActionResult LogIn(string login, string password, string remember)
        {
            ViewBag.MenuItem = MenuItem.User;
            if (login == null && password == null)
            {
                return View(new LogInResultViewModel());
            }
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
                return RedirectToAction("Index", "Home");
            }
            return View(new LogInResultViewModel {HasResult = true, Result = result});
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

        [Authorize]
        public ActionResult ChangeAvatar()
        {
            var avatar = Request.Files["avatar"];
            if (avatar != null)
            {
                var um = Loader.GetUserManager();
                var user = um.Users.Single(u => u.Id == User.Id);
                new AvatarsHelper(um).ChangeAvatar(user, avatar);
            }
            return RedirectToAction("Details", new {id = User.Id});
        }

        public JsonResult Exists(string login)
        {
            return new JsonResult {Data = new {result = Loader.GetUserManager().Exists(login)}};
        }
    }
}