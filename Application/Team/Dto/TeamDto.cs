using Mapster;
using System;

namespace Application.Team.Dto
{
    public class TeamDto : IRegister
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OfficialWebsiteURL { get; set; }
        public string LogoURL { get; set; }
        public DateTime FoundationDate { get; set; }
        //public ICollection<TournamentDto> AssignedTournaments { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Models.Team, TeamDto>();
            //.Map(dest => dest.AssignedTournaments, src => src.TournamentTeams.Select(tt => tt.Tournament).Adapt<List<TournamentDto>>().ToList());
        }
    }
}
