namespace UltraBet.Data.Models
{
    public class MatchesTeams
    {
        public string MatchId { get; set; }

        public virtual Match Match { get; set; }

        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
    }
}
