using Application.Extensions;
using Application.Team.Dto;
using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Team.Queries
{
    public class GetTeamByIdQuery : IRequest<Result>
    {
        public string Id { get; set; }

        class Handler : IRequestHandler<GetTeamByIdQuery, Result>
        {
            public class Validator : AbstractValidator<GetTeamByIdQuery>
            {
                public Validator()
                {
                    RuleFor(r => r.Id).NotEmpty().NotNull()
                        .WithMessage("Id is Required");
                }
            }
            private readonly IAppDbContext _context;
            public Handler(IAppDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
            {
                var Team = await _context.Teams
                   .Filter()
                   .ProjectToType<TeamDto>()
                   .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

                if (Team == null) return Result.Failure(ApiExeptionType.NotFound);

                return Result.Successed(Team);
            }
        }
    }
}
