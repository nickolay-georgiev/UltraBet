namespace UltraBet.Web.ViewModels
{
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class OddViewModel : IMapFrom<Odd>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public string SpecialBetValue { get; set; }
    }
}
