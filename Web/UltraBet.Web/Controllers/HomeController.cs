﻿namespace UltraBet.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using UltraBet.Services.Data;
    using UltraBet.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly ISportService sportDataService;

        public HomeController(ISportService sportDataService)
        {
            this.sportDataService = sportDataService;
        }

        public async Task<IActionResult> Index()
        {
            //await this.sportDataService.StoreDataAsync();
            this.sportDataService.GetMatchById("2014802");
            this.sportDataService.GetMatchesInNextTwentyFourHours();

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}
