using Application.Tournament.Dto;
using Common;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tournament.Queries
{
    public class GetTournamentQuery : IRequest<Result>
    {


        class Handler : IRequestHandler<GetTournamentQuery, Result>
        {
            private readonly IAppDbContext _context;
            public Handler(IAppDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(GetTournamentQuery request, CancellationToken cancellationToken)
            {
                var Tournament = await _context.Tournaments.ProjectToType<TournamentDto>().ToListAsync(cancellationToken);


                return Result.Successed(Tournament);
            }
        }
    }
}
