using Application.Team.Dto;
using Application.Tournament.Dto;
using Mapster;
using System.Collections.Generic;

namespace Application.TournamentTeam.Dto
{
    public class TournamentTeamDto : IRegister
    {
        public TournamentDto Tournament { get; set; }
        public List<TeamDto> Teams { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Models.TournamentTeam, TournamentTeamDto>();
        }
    }
}
