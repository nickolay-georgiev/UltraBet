namespace UltraBet.Web.ViewModels
{
    using AutoMapper;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class OddViewModel : IMapFrom<Odd>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public string SpecialBetValue { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Odd, OddViewModel>()
                .ForMember(x => x.Name, opt =>
                    opt.MapFrom(x => x.OddName.Name));
        }
    }
}
