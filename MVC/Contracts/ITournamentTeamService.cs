using MVC.Models;
using MVC.Services.Base;

namespace MVC.Contracts
{
    public interface ITournamentTeamService
    {
        Task<Result> AssignTournamentTeam(AssignTournamentTeamVm createVM);
        Task<List<TournamentTeamAllView>> GetAllTournamentTeams();
        Task<Result> RemoveTournamentTeam(AssignTournamentTeamVm removeVM);
    }
}
