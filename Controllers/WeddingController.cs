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

            bool existingRSVP = db.UserWeddingRSVPs
                .Any(u => u.UserId == (int)UUID);
            ViewBag.existingRSVP = existingRSVP;

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

        [HttpGet("/wedding/{weddingId}")]
        public IActionResult WeddingDetails(int weddingId)
        {
            if(!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            Wedding queriedWedding = db.Weddings
                .Include(w => w.UserWeddingRSVP)
                    .ThenInclude(u => u.User)
                    // .ThenInclude(u => u.FirstName)
                .FirstOrDefault(w => w.WeddingId == weddingId);

            return View("WeddingDetails", queriedWedding);
        }

        [HttpPost("/wedding/rsvp")]
        public IActionResult WeddingRSVP(int weddingId, UserWeddingRSVP newRSVP)
        {
            if(!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            UserWeddingRSVP existingRSVP = db.UserWeddingRSVPs
                .FirstOrDefault(u => u.UserId == (int)UUID && u.WeddingId == weddingId);

            if(existingRSVP == null)
            {
                newRSVP.WeddingId = weddingId;
                newRSVP.UserId = (int)UUID;
                db.UserWeddingRSVPs.Add(newRSVP);
            }
            else
            {
                db.UserWeddingRSVPs.Remove(existingRSVP);
            }
    
            db.SaveChanges();
                return RedirectToAction("Dashboard");
        }

        [HttpPost("/wedding/delete")]
        public IActionResult DeleteWedding(int weddingId)
        {
            if(!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Wedding deleteWedding = db.Weddings
                .SingleOrDefault(w => w.WeddingId == weddingId);
            db.Weddings.Remove(deleteWedding);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}