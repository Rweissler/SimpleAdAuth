using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using SimpleAdAuth.Data;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SimpleAdsAuth.Web.Controllers
{
    public class AccountController : Controller
    {
        public string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAdsAuth;Integrated Security=true;";
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if(TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new AdUserRepository(_connectionString);
            var user = repo.Login(email, password);
            if(user == null)
            {
                TempData["message"] = "Invalid login!";
                return Redirect("Login");
            }

            var claims = new List<Claim>
            {
                new Claim("user", email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
              new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/index");
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {
            var repo = new AdUserRepository(_connectionString);
            repo.AddUser(user, password);
            return RedirectToAction("login");
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }

    }
}
