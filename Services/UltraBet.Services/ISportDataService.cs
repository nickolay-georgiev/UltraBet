namespace UltraBet.Services
{
    using System.Threading.Tasks;

    using UltraBet.Services.Models;

    public interface ISportDataService
    {
        Task<XmlSportsDto> GetSportDataAsync(string url);
    }
}
