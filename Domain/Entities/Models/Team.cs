using Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Models
{
    public class Team : BaseEntityAudit<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string OfficialWebsiteURL { get; set; }
        public string LogoURL { get; set; }
        public string LogoPath { get; set; }
        public DateTime FoundationDate { get; set; }
        //Nav props
        public HashSet<TournamentTeam> TournamentTeams { get; set; }
        public HashSet<Player> Players { get; set; }

    }
}
