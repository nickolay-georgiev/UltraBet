namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class MatchSearchByIdViewModel : BaseMatchViewModel, IHaveCustomMappings
    {
        public ICollection<MarketInMatchSearchByIdViewModel> Markets { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Match, MatchSearchByIdViewModel>()
                .ForMember(x => x.Teams, opt =>
                    opt.MapFrom(x => x.Teams.Select(x => x.Team.Name).ToList()));
        }
    }
}
