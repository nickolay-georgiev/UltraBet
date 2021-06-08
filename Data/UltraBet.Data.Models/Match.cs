namespace UltraBet.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using UltraBet.Data.Common.Models;

    public class Match : BaseDeletableModel<string>
    {
        public Match()
        {
            this.Bets = new HashSet<Bet>();
            //this.MatchTypes = new HashSet<MatchTypes>();
        }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public int TypeId { get; set; }

        public MatchType Type { get; set; }

        //public virtual ICollection<MatchTypes> MatchTypes { get; set; }

        public string EventId { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
