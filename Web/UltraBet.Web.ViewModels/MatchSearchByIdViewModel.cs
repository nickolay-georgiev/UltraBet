namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;

    public class MatchSearchByIdViewModel : BaseMatchViewModel
    {
        public ICollection<BetInMatchSearchByIdViewModel> Bets { get; set; }
    }
}
