using MvcCalculator.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCalculator.Models;

namespace MvcCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        public ActionResult Index(bool? ignoreCookies)
        {
            ViewBag.MainMenu = MainMenuItems.Calculator;
            var cp = new CookieProcessor(HttpContext);
            if (ignoreCookies.HasValue && ignoreCookies.Value)
            {
                cp.RemoveCooke("v1");
                cp.RemoveCooke("v2");
                cp.RemoveCooke("operation");
                return View();
            }
            else if (cp.CheckCookies() == false)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Calculate");
            }
        }
        public ActionResult Calculate(double? v1, double? v2, Operation? operation, bool? store)
        {
            ViewBag.MainMenu = MainMenuItems.Calculator;
            var cp = new CookieProcessor(HttpContext);
            if (v1.HasValue && v2.HasValue && operation.HasValue)
            {
                var model = new HomeCalculateViewModel(v1.Value, v2.Value, operation.Value);

                if (!store.HasValue || store.Value)
                {
                    cp.WriteCookie("v1", v1.Value.ToString());
                    cp.WriteCookie("v2", v2.Value.ToString());
                    cp.WriteCookie("operation", operation.Value.ToString());
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        new HistoryProcessor().Add(v1.Value, v2.Value, operation.Value, HttpContext);
                    }
                }

                return View(model: model);
            }
            else
            {
                double cV1;
                double cV2;
                string operationStr = cp.ReadCookie("operation");
                Operation? cOperation;
                if (operationStr != null)
                {
                    cOperation = (Operation?)Enum.Parse(typeof(Operation), operationStr);
                }
                if (Double.TryParse(cp.ReadCookie("v1"), out cV1)
                    && Double.TryParse(cp.ReadCookie("v2"), out cV2)
                    && operationStr != null && (cOperation = (Operation?)Enum.Parse(typeof(Operation), operationStr)).HasValue)
                {
                    var model = new HomeCalculateViewModel(cV1, cV2, cOperation.Value);
                    return View(model: model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult History()
        {
            ViewBag.MainMenu = MainMenuItems.Calculator;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View(new HistoryProcessor().GetHistory(HttpContext.User.Identity.Name));
            }
            else
            {
                return View(new HistoryProcessor().GetHistory(null));
            }
        }
    }
}
