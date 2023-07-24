using Application.Team.Dto;
using Common;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Team.Queries
{
    public class GetTeamQuery : IRequest<Result>
    {

        class Handler : IRequestHandler<GetTeamQuery, Result>
        {
            private readonly IAppDbContext _context;
            public Handler(IAppDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(GetTeamQuery request, CancellationToken cancellationToken)
            {
                var Team = await _context.Teams.ProjectToType<TeamDto>().ToListAsync(cancellationToken);

                return Result.Successed(Team);
            }
        }
    }
}
