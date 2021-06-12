namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class MarketName : BaseModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Market> Markets { get; set; }
    }
}
