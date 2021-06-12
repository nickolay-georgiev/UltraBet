namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;

    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class BaseMarketViewModel : IMapFrom<Market>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public ICollection<OddViewModel> Odds { get; set; }
    }
}
