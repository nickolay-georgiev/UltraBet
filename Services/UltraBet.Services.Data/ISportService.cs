namespace UltraBet.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UltraBet.Services.Models;
    using UltraBet.Web.ViewModels;

    public interface ISportService
    {
        Task StoreDataAsync(XmlSportsDto input);

        IEnumerable<MatchInNext24HoursViewModel> GetMatchesInNextTwentyFourHours();

        MatchSearchByIdViewModel GetMatchById(string id);
    }
}
