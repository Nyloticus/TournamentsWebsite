using Common;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Team.Commands
{
    public class DeleteTeamCommand : IRequest<Result>
    {
        public string Id { get; set; }

        public class Validator : AbstractValidator<DeleteTeamCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty()
                    .WithMessage("Id is required");
            }
        }

        public class Handler : IRequestHandler<DeleteTeamCommand, Result>
        {
            private readonly ITeamRepository _teamRepository;
            public Handler(ITeamRepository teamRepository)
            {
                _teamRepository = teamRepository;
            }

            public async Task<Result> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
            {
                var Team = await _teamRepository.GetByIdAsync(request.Id);
                if (!Team.Success)
                    return Team;

                //delete logo
                var oldpath = Team.Payload.LogoPath;
                if (oldpath != null)
                {
                    FileInfo file = new FileInfo(oldpath);
                    if (file.Exists) file.Delete();
                }

                var result = await _teamRepository.DeleteAsync(Team.Payload);
                return Result.Successed("Deleted Successfully");

            }
        }
    }
}
