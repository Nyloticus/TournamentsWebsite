using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class TeamVM : CreateTeamVm
    {
        public string Id { get; set; }
    }
    public class CreateTeamVm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string OfficialWebsiteURL { get; set; }
        [Required]
        public IFormFile Logo { get; set; }
        [Required]
        public DateTime FoundationDate { get; set; }
    }
    public class ViewTeam
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OfficialWebsiteURL { get; set; }
        public string LogoURL { get; set; }
        public DateTime FoundationDate { get; set; }
    }
}
