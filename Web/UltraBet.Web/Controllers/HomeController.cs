namespace UltraBet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using UltraBet.Common;
    using UltraBet.Services;
    using UltraBet.Services.Data;

    public class HomeController : BaseController
    {
        private readonly ISportService sportService;
        private readonly ISportDataService sportDataService;

        public HomeController(ISportService sportService, ISportDataService sportDataService)
        {
            this.sportService = sportService;
            this.sportDataService = sportDataService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await this.sportDataService.GetSportDataAsync(GlobalConstants.SportDataUrl);

            await this.sportService.StoreDataAsync(data);
            this.sportService.GetMatchesInNextTwentyFourHours();
            // this.sportService.GetMatchById("2014802");

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
