using Application.Team.Dto;
using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Team.Queries
{
    public class GetUnassignedTeamsQuery : IRequest<Result>
    {
        public string TournamentId { get; set; }

        class Handler : IRequestHandler<GetUnassignedTeamsQuery, Result>
        {
            public class Validator : AbstractValidator<GetUnassignedTeamsQuery>
            {
                public Validator()
                {
                    RuleFor(r => r.TournamentId).NotEmpty().NotNull()
                        .WithMessage("TournamentId is Required");
                }
            }
            private readonly IAppDbContext _context;
            public Handler(IAppDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(GetUnassignedTeamsQuery request, CancellationToken cancellationToken)
            {
                var notAssignedTeams = _context.Teams
                          .Where(team => !_context.TournamentTeams
                          .Any(tt => tt.TournamentId == request.TournamentId && tt.TeamId == team.Id))
                          .ProjectToType<TeamDto>()
                          .ToList();

                return Result.Successed(notAssignedTeams);
            }
        }
    }
}
