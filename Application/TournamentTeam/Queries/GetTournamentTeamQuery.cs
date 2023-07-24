using Application.Team.Dto;
using Application.Tournament.Dto;
using Application.TournamentTeam.Dto;
using Common;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TournamentTeamTeam.Queries
{
    public class GetTournamentTeamQuery : IRequest<Result>
    {
        class Handler : IRequestHandler<GetTournamentTeamQuery, Result>
        {
            private readonly IAppDbContext _context;
            public Handler(IAppDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(GetTournamentTeamQuery request, CancellationToken cancellationToken)
            {
                var TournamentTeamLs = _context.Tournaments.Select(t => new TournamentTeamDto
                {
                    Tournament = t.Adapt<TournamentDto>(),
                    Teams = t.TournamentTeams.Select(tt => tt.Team).Adapt<List<TeamDto>>().ToList()
                }).ToList();

                return Result.Successed(TournamentTeamLs);
            }
        }
    }
}
