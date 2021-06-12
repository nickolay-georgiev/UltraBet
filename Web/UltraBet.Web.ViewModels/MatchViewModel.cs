namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class MatchViewModel : BaseMatchViewModel, IHaveCustomMappings
    {
        public ICollection<MarketViewModel> Markets { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Match, MatchViewModel>()
                .ForMember(x => x.Markets, opt =>
                    opt.MapFrom(x => x.Markets
                       .Where(x => x.MarketName.Name == "Match Winner" ||
                                   x.MarketName.Name == "Map Advantage" ||
                                   x.MarketName.Name == "Total Maps Played")))
                .ForMember(x => x.Teams, opt =>
                    opt.MapFrom(x => x.Teams.Select(x => x.Team.Name).ToList()));
        }
    }
}
