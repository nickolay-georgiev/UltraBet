namespace UltraBet.Web.ViewModels
{
    using AutoMapper;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class MarketInMatchSearchByIdViewModel : BaseMarketViewModel, IHaveCustomMappings
    {
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Market, MarketInMatchSearchByIdViewModel>()
                .ForMember(x => x.Name, opt =>
                    opt.MapFrom(x => x.MarketName.Name));
        }
    }
}
