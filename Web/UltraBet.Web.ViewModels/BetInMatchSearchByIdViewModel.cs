namespace UltraBet.Web.ViewModels
{
    using AutoMapper;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class BetInMatchSearchByIdViewModel : BaseBetViewModel, IHaveCustomMappings
    {
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Bet, BetInMatchSearchByIdViewModel>()
                .ForMember(x => x.Name, opt =>
                    opt.MapFrom(x => x.BetName.Name));
        }
    }
}
