using MVC.Models;
using MVC.Services.Base;

namespace MVC.Contracts
{
    public interface ITeamService
    {
        Task<List<ViewTeam>> GetAllTeams();
        Task<Result> AddTeam(CreateTeamVm createVM);
        Task<Result> DeleteTeam(string teamId);
        Task<ViewTeam> GetTeamById(string id);
        Task<Result> UpdateTeam(TeamVM editVM);
        Task<List<ViewTeam>> GetUnassignedTeams(string TournamentId);

    }
}
