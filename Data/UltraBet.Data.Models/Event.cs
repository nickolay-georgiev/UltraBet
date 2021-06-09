namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class Event : BaseDeletableModel<string>
    {
        public Event()
        {
            this.Matches = new HashSet<Match>();
        }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public string CategoryId { get; set; }

        public string SportId { get; set; }

        public virtual Sport Sport { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
