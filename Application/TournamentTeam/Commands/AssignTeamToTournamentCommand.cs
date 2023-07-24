using Common;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TournamentTeam.Commands
{
    public class AssignTeamToTournamentCommand : IRequest<Result>
    {
        public string TournamentId { get; set; }
        public List<string> TeamIds { get; set; }

        public class Validator : AbstractValidator<AssignTeamToTournamentCommand>
        {
            public Validator()
            {
                RuleFor(r => r.TournamentId).NotEmpty()
                    .WithMessage("Tournament ID is required");
                RuleFor(r => r.TeamIds).NotNull().Must(r => r.Count > 0)
                    .WithMessage("At least one team has to be selected");
            }
        }
        public class Handler : IRequestHandler<AssignTeamToTournamentCommand, Result>
        {
            private readonly ITournamentTeamRepository _tournamentTeamRepository;
            private readonly ITournamentRepository _tournamentRepository;
            private readonly ITeamRepository _teamRepository;
            public Handler(ITournamentTeamRepository tournamentTeamRepository, ITeamRepository teamRepository, ITournamentRepository tournamentRepository)
            {
                _tournamentTeamRepository = tournamentTeamRepository;
                _teamRepository = teamRepository;
                _tournamentRepository = tournamentRepository;
            }

            public async Task<Result> Handle(AssignTeamToTournamentCommand request, CancellationToken cancellationToken)
            {
                var Tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId);
                if (!Tournament.Success)
                    return Tournament;

                var Teams = await _teamRepository.GetByMultipleIdsAsync(request.TeamIds);
                if (!Teams.Success)
                    return Teams;

                var TournamentTeams = request.TeamIds.Select(t => MapTournamentTeam(request.TournamentId, t));

                var result = await _tournamentTeamRepository.AddRangeAsync(TournamentTeams.ToArray());

                return Result.Successed("Added Successfully");
            }
            public Domain.Entities.Models.TournamentTeam MapTournamentTeam(string tournamentId, string teamId) =>
                new Domain.Entities.Models.TournamentTeam
                {
                    TeamId = teamId,
                    TournamentId = tournamentId
                };
        }

    }
}
