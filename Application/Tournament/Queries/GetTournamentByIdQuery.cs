using Application.Extensions;
using Application.Tournament.Dto;
using Common;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tournament.Queries
{
    public class GetTournamentByIdQuery : IRequest<Result>
    {
        public string Id { get; set; }

        class Handler : IRequestHandler<GetTournamentByIdQuery, Result>
        {
            public class Validator : AbstractValidator<GetTournamentByIdQuery>
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

            public async Task<Result> Handle(GetTournamentByIdQuery request, CancellationToken cancellationToken)
            {
                var Tournament = await _context.Tournaments
                   .Filter()
                   .ProjectToType<TournamentDto>()
                   .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

                if (Tournament == null) return Result.Failure(ApiExeptionType.NotFound);

                return Result.Successed(Tournament);
            }
        }
    }
}
