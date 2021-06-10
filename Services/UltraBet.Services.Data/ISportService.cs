namespace UltraBet.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UltraBet.Web.ViewModels;

    public interface ISportService
    {
        Task StoreDataAsync();

        IEnumerable<MatchViewModel> GetMatchesInNextTwentyFourHours();

        MatchSearchByIdViewModel GetMatchById(string id);
    }
}
