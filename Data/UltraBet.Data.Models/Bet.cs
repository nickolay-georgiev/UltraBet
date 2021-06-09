namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class Bet : BaseDeletableModel<string>
    {
        public Bet()
        {
            this.Odds = new HashSet<Odd>();
        }

        public bool IsLive { get; set; }

        public string MatchId { get; set; }

        public virtual Match Match { get; set; }

        public int BetNameId { get; set; }

        public virtual BetName BetName { get; set; }

        public virtual ICollection<Odd> Odds { get; set; }
    }
}
