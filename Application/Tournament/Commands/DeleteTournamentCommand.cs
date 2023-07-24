using Common;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tournament.Commands
{
    public class DeleteTournamentCommand : IRequest<Result>
    {
        public string Id { get; set; }

        public class Validator : AbstractValidator<DeleteTournamentCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty()
                    .WithMessage("Id is required");
            }
        }

        public class Handler : IRequestHandler<DeleteTournamentCommand, Result>
        {
            private readonly ITournamentRepository _tournamentRepository;
            public Handler(ITournamentRepository tournamentRepository)
            {
                _tournamentRepository = tournamentRepository;
            }

            public async Task<Result> Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
            {
                var Tournament = await _tournamentRepository.GetByIdAsync(request.Id);
                if (!Tournament.Success)
                    return Tournament;

                //delete logo
                var oldpath = Tournament.Payload.LogoPath;
                if (oldpath != null)
                {
                    FileInfo file = new FileInfo(oldpath);
                    if (file.Exists) file.Delete();
                }

                var result = await _tournamentRepository.DeleteAsync(Tournament.Payload);
                return Result.Successed("Deleted Successfully");

            }
        }
    }
}
