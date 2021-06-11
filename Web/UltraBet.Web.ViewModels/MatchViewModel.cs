namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class MatchViewModel : BaseMatchViewModel, IHaveCustomMappings
    {
        public ICollection<BetViewModel> Bets { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Match, MatchViewModel>()
                .ForMember(x => x.Bets, opt =>
                    opt.MapFrom(x => x.Bets
                       .Where(x => x.BetName.Name == "Match Winner" ||
                                   x.BetName.Name == "Map Advantage" ||
                                   x.BetName.Name == "Total Maps Played")))
                .ForMember(x => x.Teams, opt =>
                    opt.MapFrom(x => x.Teams.Select(x => x.Team.Name).ToList()));
        }
    }
}
