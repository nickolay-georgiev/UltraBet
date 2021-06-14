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
        private readonly IFetchSportDataService fetchSportDataService;

        public HomeController(ISportService sportService, IFetchSportDataService fetchSportDataService)
        {
            this.sportService = sportService;
            this.fetchSportDataService = fetchSportDataService;
        }

        public async Task<IActionResult> Index()
        {
            //var data = await this.fetchSportDataService.GetSportDataAsync(GlobalConstants.SportDataUrl);

            //await this.sportService.StoreDataAsync(data);
            //this.sportService.GetMatchesInNextTwentyFourHours();
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
