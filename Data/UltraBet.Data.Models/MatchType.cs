using System;
using System.Collections.Generic;
using System.Text;
using UltraBet.Data.Common.Models;

namespace UltraBet.Data.Models
{
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
