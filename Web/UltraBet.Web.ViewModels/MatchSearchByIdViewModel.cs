namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;

    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class MatchSearchByIdViewModel : BaseMatchViewModel, IMapFrom<Match>
    {
        public ICollection<BetInMatchSearchByIdViewModel> Bets { get; set; }
    }
}
