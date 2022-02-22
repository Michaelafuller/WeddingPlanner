using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var allWeddings = db.Weddings
                .Include(w => w.UserWeddingRSVP)
                    .ThenInclude(u => u.User)
                .ToList();
            ViewBag.allWeddings = allWeddings;

            return View("Dashboard");
        }

        [HttpGet("/wedding/new")]
        public IActionResult NewWedding()
        {
            if(!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("NewWedding");
        }

        [HttpPost("/wedding/create")]
        public IActionResult CreateWedding(Wedding newWedding)
        {
            if(!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            newWedding.CreatedBy = (int)UUID;
            db.Add(newWedding);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}