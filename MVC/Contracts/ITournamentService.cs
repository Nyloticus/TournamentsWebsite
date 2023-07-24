using MVC.Models;
using MVC.Services.Base;

namespace MVC.Contracts
{
    public interface ITournamentService
    {
        Task<List<ViewTournament>> GetAllTournaments();
        Task<Result> AddTournament(CreateTournamentVm createVM);
        Task<Result> DeleteTournament(string TournamentId);
        Task<ViewTournament> GetTournamentById(string id);
        Task<Result> UpdateTournament(TournamentVM editVM);
        string GetYouTubeVideoId(string videoLink);
    }
}
