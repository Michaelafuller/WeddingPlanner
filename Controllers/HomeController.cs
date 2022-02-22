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
    public class HomeController : Controller
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
        public HomeController(WeddingPlannerContext context)
        {
            db = context;
        }
    

    [HttpGet("/")]
    public IActionResult Index()
    {
        if(isLoggedIn)
        {
            return RedirectToAction("Dashboard", "Wedding");
        }
        return View("Index");
    }

    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {
        if(ModelState.IsValid == false)
        {
            return View("Index");
        }

        if(db.Users.Any(u => u.Email == newUser.Email))
        {
            ModelState.AddModelError("Email", "Email already in use!");
            return View("Index");
        }

        PasswordHasher<User> hasher = new PasswordHasher<User>();
        newUser.Password = hasher.HashPassword(newUser, newUser.Password);

        db.Add(newUser);
        db.SaveChanges();

        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        return RedirectToAction("Dashboard", "Wedding");
    }

        [HttpPost("/login/user")]
        public IActionResult Login(LoginUser loginUser)
        {
            if(ModelState.IsValid == false)
            {
                return View("Index");
            }

            if(ModelState.IsValid)
            {
                var userInDb = db.Users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);
                if (userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UUID", userInDb.UserId);
            }
                return RedirectToAction("Dashboard", "Wedding");
        }

        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            if (isLoggedIn)
            {
                HttpContext.Session.Remove("UUID");
            }
            return RedirectToAction("Index", "Home");
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
}
