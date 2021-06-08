namespace UltraBet.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using UltraBet.Common;
    using UltraBet.Data.Models;
    using UltraBet.Services.Models;

    public class SportDataService : ISportDataService
    {
        private const string XmlRootAttribute = "XmlSports";

        private readonly HttpClient httpClient;
        private readonly IDeserializer deserializer;

        public SportDataService(
            HttpClient httpClient,
            IDeserializer deserializer)
        {
            this.httpClient = httpClient;
            this.deserializer = deserializer;
        }

        public async Task<XmlSportsDto> GetSportDataAsync()
        {
            var response = await this.httpClient.GetAsync(GlobalConstants.SportDataUrl);
            var responseAsString = await response.Content.ReadAsStringAsync();

            var data = this.deserializer.Deserialize<XmlSportsDto>(responseAsString, XmlRootAttribute);

            return data;
        }
    }
}
