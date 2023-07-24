using Mapster;

namespace Application.Tournament.Dto
{
    public class TournamentDto : IRegister
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoURL { get; set; }
        public string TournamentVideo { get; set; }
        //public ICollection<TeamDto> AssignedTeams { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Models.Tournament, TournamentDto>();
        }
    }
}
