using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using SimpleAdsAuth.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SimpleAdAuth.Data;

namespace SimpleAdsAuth.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAdsAuth;Integrated Security=true;";

        //The below Index I dont understand fully.
       public IActionResult Index()
        {
            var repo = new AdUserRepository(_connectionString);
            var ads = repo.GetAds();
            var vm = new HomePageViewModel();
            if(!User.Identity.IsAuthenticated)
            {
                vm.AdViewModels = ads.Select(a => new AdViewModel { Ad = a }).ToList();
                return View(vm);
            }
            var user = repo.GetByEmail(User.Identity.Name);
            vm.AdViewModels = ads.Select(a =>
            {
                return new AdViewModel
                {
                    Ad = a,
                    CanDelete = user.Id == a.UserId
                };
            }).ToList();
            return View(vm);
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            var repo = new AdsUsersRepository(_connectionString);
            return View(new MyAccountViewModel
            {
                Ads = repo.GetAdsByUser(User.Identity.Name)
            });
        }

        [HttpPost]
        public IActionResult DeleteAd(int id, string currentPage)
        {
            var repo = new AdsUsersRepository(_connectionString);
            repo.DeletAds(id);
            return RedirectToAction(currentPage);
        }


        private object AdUserRepository(string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
