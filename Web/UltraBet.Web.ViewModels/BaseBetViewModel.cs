namespace UltraBet.Web.ViewModels
{
    using System.Collections.Generic;

    public class BaseBetViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public ICollection<OddViewModel> Odds { get; set; }
    }
}
