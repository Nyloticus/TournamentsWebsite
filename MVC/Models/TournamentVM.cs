using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class TournamentVM : CreateTournamentVm
    {
        public string Id { get; set; }
    }
    public class CreateTournamentVm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Logo { get; set; }
        [Required]
        public string TournamentVideo { get; set; }
    }
    public class ViewTournament
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoURL { get; set; }
        public string TournamentVideo
        {
            get; set;
        }
    }
}
