namespace UltraBet.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using UltraBet.Services.Models;

    public class FetchSportDataService : IFetchSportDataService
    {
        private readonly HttpClient httpClient;
        private readonly ISerializationService serializationService;

        public FetchSportDataService(
            HttpClient httpClient,
            ISerializationService serializationService)
        {
            this.httpClient = httpClient;
            this.serializationService = serializationService;
        }

        public async Task<XmlSportsDto> GetSportDataAsync(string url)
        {
            var response = await this.httpClient.GetAsync(url);
            var responseAsString = await response.Content.ReadAsStringAsync();

            var data = this.serializationService.DeserializeSportData<XmlSportsDto>(responseAsString);

            return data;
        }
    }
}
