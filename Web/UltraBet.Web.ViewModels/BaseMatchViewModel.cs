namespace UltraBet.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class BaseMatchViewModel : IMapFrom<Match>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> Teams { get; set; }

        public DateTime StartDate { get; set; }
    }
}
