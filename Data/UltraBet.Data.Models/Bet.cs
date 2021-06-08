namespace UltraBet.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using UltraBet.Data.Common.Models;

    public class Bet : BaseDeletableModel<string>
    {
        public Bet()
        {
            this.Odds = new HashSet<Odd>();
        }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public string MatchId { get; set; }

        public virtual Match Match { get; set; }

        public virtual ICollection<Odd> Odds { get; set; }
    }
}
