namespace UltraBet.Data.Models
{
    using UltraBet.Data.Common.Models;

    public class Odd : BaseDeletableModel<string>
    {
        public double Value { get; set; }

        public int? GroupNumber { get; set; }

        public string SpecialBetValue { get; set; }

        public int OddNameId { get; set; }

        public virtual OddName OddName { get; set; }

        public string MarketId { get; set; }

        public virtual Market Bet { get; set; }
    }
}
