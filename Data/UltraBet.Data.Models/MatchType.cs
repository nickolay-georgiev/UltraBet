namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class MatchType : BaseModel<int>
    {
        public MatchType()
        {
            this.Matches = new HashSet<Match>();
        }

        public string Name { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
