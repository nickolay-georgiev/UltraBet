namespace UltraBet.Services.Data.CronJobs
{
    using System.Threading.Tasks;

    using UltraBet.Common;

    public class FetchSportData
    {
        private readonly ISportService sportService;
        private readonly IFetchSportDataService fetchSportDataService;

        public FetchSportData(
            ISportService sportService,
            IFetchSportDataService fetchSportDataService)
        {
            this.sportService = sportService;
            this.fetchSportDataService = fetchSportDataService;
        }

        public async Task Work()
        {
            var data = await this.fetchSportDataService.GetSportDataAsync(GlobalConstants.SportDataUrl);
            await this.sportService.StoreDataAsync(data);
        }
    }
}
