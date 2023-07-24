using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{

    public class AssignTournamentTeamVm
    {
        [Required]
        public string TournamentId { get; set; }
        [Required]
        public List<string> TeamIds { get; set; }
    }
    public class TournamentTeamView
    {
        public List<ViewTeam> Teams { get; set; }
        public List<ViewTournament> Tournaments { get; set; }
    }
    public class TournamentTeamAllView
    {
        public List<ViewTeam> Teams { get; set; }
        public ViewTournament Tournament { get; set; }
    }
}
