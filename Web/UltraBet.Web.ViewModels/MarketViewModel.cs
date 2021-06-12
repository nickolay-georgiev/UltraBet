namespace UltraBet.Web.ViewModels
{
    using System.Linq;

    using AutoMapper;
    using UltraBet.Common;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class MarketViewModel : BaseMarketViewModel, IHaveCustomMappings
    {
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Market, MarketViewModel>()
                .ForMember(x => x.Odds, opt =>
                    opt.MapFrom(x => x.Odds
                       .Where(x => x.GroupNumber == GlobalConstants.DefaultGroupNumber)))
                .ForMember(x => x.Name, opt =>
                    opt.MapFrom(x => x.MarketName.Name));
        }
    }
}
