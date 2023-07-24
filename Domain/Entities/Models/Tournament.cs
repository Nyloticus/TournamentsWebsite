using Common;
using System.Collections.Generic;

namespace Domain.Entities.Models
{
    public class Tournament : BaseEntityAudit<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoURL { get; set; }
        public string LogoPath { get; set; }
        public string TournamentVideo { get; set; }
        //Nav props
        public HashSet<TournamentTeam> TournamentTeams { get; set; }
    }
}
