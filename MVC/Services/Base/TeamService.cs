using MVC.Contracts;
using MVC.Models;
using System.Net.Http.Headers;

namespace MVC.Services.Base
{
    public class TeamService : ITeamService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly string BasePath;

        public TeamService(HttpClient httpClient, ILocalStorageService localStorage, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));

            BasePath = configuration.GetValue<string>("BasePath") + "api/Team/";
        }

        public async Task<List<ViewTeam>> GetAllTeams()
        {
            var response = await _httpClient.GetAsync(BasePath + "all");
            return await response.ReadContentListAsync<ViewTeam>();
        }
        public async Task<List<ViewTeam>> GetUnassignedTeams(string TournamentId)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(BasePath + "unassigned?TournamentId=" + TournamentId);
            return await response.ReadContentListAsync<ViewTeam>();

        }
        public async Task<Result> AddTeam(CreateTeamVm createVM)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formDataContent = new MultipartFormDataContent();

            formDataContent.Add(new StringContent(createVM.Name), "name");
            formDataContent.Add(new StringContent(createVM.Description), "description");
            formDataContent.Add(new StringContent(createVM.OfficialWebsiteURL), "officialWebsiteURL");
            formDataContent.Add(new StringContent(createVM.FoundationDate.ToString("yyyy-MM-dd")), "foundationDate");

            if (createVM.Logo != null)
            {
                string fileExtension = Path.GetExtension(createVM.Logo.FileName)?.ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                {
                    Result fm = new Result();
                    fm.Message = "Invalid file extension. Only .jpg, .jpeg, and .png files are allowed.";
                    return fm;
                }

                formDataContent.Add(new StreamContent(createVM.Logo.OpenReadStream()), "Logo", createVM.Logo.FileName);
            }

            var response = await _httpClient.PostAsync(BasePath + "create", formDataContent);
            var result = await response.ReadFromResultAsync<Result>();
            return result;
        }
        public async Task<Result> DeleteTeam(string teamId)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync(BasePath + $"remove/{teamId}");
            var result = await response.ReadFromResultAsync<Result>();
            return result;
        }
        public async Task<ViewTeam> GetTeamById(string id)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(BasePath + "find/" + id);
            
            if (response.IsSuccessStatusCode)
            {
                var team = await response.ReadContentAsync<ViewTeam>();
                return team;
            }
            else
            {
                return null;
            }
        }
        public async Task<Result> UpdateTeam(TeamVM editVM)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formDataContent = new MultipartFormDataContent();

            formDataContent.Add(new StringContent(editVM.Id), "id");
            formDataContent.Add(new StringContent(editVM.Name), "name");
            formDataContent.Add(new StringContent(editVM.Description), "description");
            formDataContent.Add(new StringContent(editVM.OfficialWebsiteURL), "officialWebsiteURL");
            formDataContent.Add(new StringContent(editVM.FoundationDate.ToString("yyyy-MM-dd")), "foundationDate");

            if (editVM.Logo != null)
            {
                string fileExtension = Path.GetExtension(editVM.Logo.FileName)?.ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                {
                    Result fm = new Result();
                    fm.Message = "Invalid file extension. Only .jpg, .jpeg, and .png files are allowed.";
                    return fm;
                }

                formDataContent.Add(new StreamContent(editVM.Logo.OpenReadStream()), "Logo", editVM.Logo.FileName);
            }

            var response = await _httpClient.PutAsync(BasePath + "edit", formDataContent);
            var result = await response.ReadFromResultAsync<Result>();
            return result;
        }


    }
}
