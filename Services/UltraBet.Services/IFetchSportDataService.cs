namespace UltraBet.Services
{
    using System.Threading.Tasks;

    using UltraBet.Services.Models;

    public interface IFetchSportDataService
    {
        Task<XmlSportsDto> GetSportDataAsync(string url);
    }
}
