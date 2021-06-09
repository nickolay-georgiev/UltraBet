namespace UltraBet.Data.Models
{

    using UltraBet.Data.Common.Models;

    public class Odd : BaseDeletableModel<string>
    {
        public string Name { get; set; }

        public double Value { get; set; }

        public int? GroupNumber { get; set; }

        public string SpecialBetValue { get; set; }

        public string BetId { get; set; }

        public virtual Bet Bet { get; set; }
    }
}
