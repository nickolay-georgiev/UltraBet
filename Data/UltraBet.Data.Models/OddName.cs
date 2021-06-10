namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class OddName : BaseModel<int>
    {
        public OddName()
        {
            this.Odds = new HashSet<Odd>();
        }

        public string Name { get; set; }

        public virtual ICollection<Odd> Odds { get; set; }
    }
}
