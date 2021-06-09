namespace UltraBet.Data.Models
{
    using System;
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class Match : BaseDeletableModel<string>
    {
        public Match()
        {
            this.Bets = new HashSet<Bet>();
            this.Teams = new HashSet<MatchesTeams>();
        }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public int TypeId { get; set; }

        public MatchType Type { get; set; }

        public string EventId { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }

        public virtual ICollection<MatchesTeams> Teams { get; set; }
    }
}
