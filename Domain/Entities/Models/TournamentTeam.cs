using Common;

namespace Domain.Entities.Models
{
    public class TournamentTeam : BaseEntityAudit<string>
    {
        //Tournament nav
        public string TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        //Team nav
        public string TeamId { get; set; }
        public Team Team { get; set; }
    }
}
