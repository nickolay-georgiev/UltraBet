namespace UltraBet.Services.Data.CronJobs
{
    using System.Threading.Tasks;

    using UltraBet.Common;

    public class FetchSportData
    {
        private readonly ISportService sportService;
        private readonly ISportDataService sportDataService;

        public FetchSportData(
            ISportService sportService,
            ISportDataService sportDataService)
        {
            this.sportService = sportService;
            this.sportDataService = sportDataService;
        }

        public async Task Work()
        {
            var data = await this.sportDataService.GetSportDataAsync(GlobalConstants.SportDataUrl);
            await this.sportService.StoreDataAsync(data);
        }
    }
}
