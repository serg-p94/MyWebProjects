﻿using System;
using System.Web.Mvc;
using WebMonitoringAppLive.Helper;

namespace WebMonitoringAppLive.Controllers
{
    public class ResourcesController : Controller
    {
        //
        // GET: /Resources/

        public ActionResult Index()
        {
            ViewBag.MenuItem = MenuItem.Home;
            return View();
        }

        public ActionResult Memory()
        {
            ViewBag.MenuItem = MenuItem.Memory;
            return
                View(Bootstrapper.Bootstrapper.GetMemoryStatisticsLoader()
                    .LoadMemoryStatistics(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));
        }

        public ActionResult Hdd()
        {
            ViewBag.MenuItem = MenuItem.Hdd;
            return
                View(Bootstrapper.Bootstrapper.GetHddStatisticsLoader()
                    .LoadHddStatistics(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));
        }

    }
}
