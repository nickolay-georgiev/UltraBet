namespace UltraBet.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using UltraBet.Services.Models;

    public class SportDataService : ISportDataService
    {
        private readonly HttpClient httpClient;
        private readonly IDeserializer deserializer;

        public SportDataService(
            HttpClient httpClient,
            IDeserializer deserializer)
        {
            this.httpClient = httpClient;
            this.deserializer = deserializer;
        }

        public async Task<XmlSportsDto> GetSportDataAsync(string url)
        {
            var response = await this.httpClient.GetAsync(url);
            var responseAsString = await response.Content.ReadAsStringAsync();

            var data = this.deserializer.Deserialize<XmlSportsDto>(responseAsString);

            return data;
        }
    }
}
