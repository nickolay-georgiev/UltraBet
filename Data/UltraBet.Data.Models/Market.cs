namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class Market : BaseDeletableModel<string>
    {
        public Market()
        {
            this.Odds = new HashSet<Odd>();
        }

        public bool IsLive { get; set; }

        public string MatchId { get; set; }

        public virtual Match Match { get; set; }

        public int MarketNameId { get; set; }

        public virtual MarketName MarketName { get; set; }

        public virtual ICollection<Odd> Odds { get; set; }
    }
}
