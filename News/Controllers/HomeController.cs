﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using News.Common.TempData;
using News.Models;

namespace News.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult GetMenu()
        {
            var model = new MenuModel();
            var listMenu = new TemplateData().ListMenu;
            model = listMenu;

            return PartialView("_MenuPartial", model);
        }
    }
}