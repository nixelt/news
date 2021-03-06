﻿using System.Web.Mvc;

namespace News24.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
    }
}