namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class Team : BaseDeletableModel<int>
    {
        public Team()
        {
            this.Matches = new HashSet<MatchesTeams>();
        }

        public string Name { get; set; }

        public virtual ICollection<MatchesTeams> Matches { get; set; }
    }
}
