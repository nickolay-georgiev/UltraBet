namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class BetName : BaseModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
