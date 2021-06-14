namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using UltraBet.Common;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class MatchInNext24HoursViewModel : BaseMatchViewModel, IHaveCustomMappings
    {
        public ICollection<MarketInMatchInNex24HourViewModel> Markets { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Match, MatchInNext24HoursViewModel>()
                .ForMember(x => x.Markets, opt =>
                    opt.MapFrom(x => x.Markets
                       .Where(x => x.MarketName.Name == GlobalConstants.MarketMatchWinner ||
                                   x.MarketName.Name == GlobalConstants.MarketMapAdvantage ||
                                   x.MarketName.Name == GlobalConstants.MarketTotalMapsPlayed)))
                .ForMember(x => x.Teams, opt =>
                    opt.MapFrom(x => x.Teams.Select(x => x.Team.Name).ToList()));
        }
    }
}
