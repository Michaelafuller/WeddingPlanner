using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller
    {
        private int? UUID
        {
            get
            {
                return HttpContext.Session.GetInt32("UUID");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return UUID != null;
            }
        }
        private WeddingPlannerContext db;
        public WeddingController(WeddingPlannerContext context)
        {
            db = context;
        }

        [HttpGet("/dashboard")]
        public IActionResult Dashboard()
        {
            if(!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Dashboard");
        }

    }
}