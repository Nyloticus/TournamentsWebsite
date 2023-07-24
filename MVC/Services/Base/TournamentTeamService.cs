using MVC.Contracts;
using MVC.Models;
using System.Net.Http.Headers;

namespace MVC.Services.Base
{
    public class TournamentTeamService : ITournamentTeamService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly string BasePath;

        public TournamentTeamService(HttpClient httpClient, ILocalStorageService localStorage, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            BasePath = configuration.GetValue<string>("BasePath") + "api/TournamentTeam/";
        }

        public async Task<List<TournamentTeamAllView>> GetAllTournamentTeams()
        {
            var response = await _httpClient.GetAsync(BasePath + "all");
            return await response.ReadContentListAsync<TournamentTeamAllView>();
        }
        public async Task<Result> AssignTournamentTeam(AssignTournamentTeamVm createVM)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var response = await _httpClient.PostAsJsonAsync(BasePath + "assign", createVM);
            var result = await response.ReadFromResultAsync<Result>();
            return result;
        }
        public async Task<Result> RemoveTournamentTeam(AssignTournamentTeamVm createVM)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync(BasePath + "remove-assign", createVM);
            var result = await response.ReadFromResultAsync<Result>();
            return result;
        }
    }
}
