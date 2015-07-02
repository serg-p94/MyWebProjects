using MvcCalculator.Models;
using MvcCalculator.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcCalculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MainMenu = MainMenuItems.Home;
            return View();
        }

        public ActionResult ShowMessage(Message msg)
        {
            return View(msg);
        }
    }
}
