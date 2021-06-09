namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;

    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class SingleMatchSearchViewModel : BaseMatchViewModel, IMapFrom<Match>
    {
        public ICollection<BetInSingleMatchSearchViewModel> Bets { get; set; }
    }
}
