using MVC.Contracts;
using MVC.Models;
using System.Net.Http.Headers;
using System.Web;

namespace MVC.Services.Base
{
    public class TournamentService : ITournamentService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly string BasePath;

        public TournamentService(HttpClient httpClient, ILocalStorageService localStorage, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            BasePath = configuration.GetValue<string>("BasePath") + "api/Tournament/";
        }

        public async Task<List<ViewTournament>> GetAllTournaments()
        {
            var response = await _httpClient.GetAsync(BasePath + "all");
            return await response.ReadContentListAsync<ViewTournament>();
        }
        public async Task<Result> AddTournament(CreateTournamentVm createVM)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formDataContent = new MultipartFormDataContent();

            formDataContent.Add(new StringContent(createVM.Name), "name");
            formDataContent.Add(new StringContent(createVM.Description), "description");
            formDataContent.Add(new StringContent(createVM.TournamentVideo), "tournamentVideo");


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
        public async Task<Result> DeleteTournament(string TournamentId)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync(BasePath + $"remove/{TournamentId}");
            var result = await response.ReadFromResultAsync<Result>();
            return result;
        }
        public async Task<ViewTournament> GetTournamentById(string id)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(BasePath + "find/" + id);

            if (response.IsSuccessStatusCode)
            {
                var Tournament = await response.ReadContentAsync<ViewTournament>();
                return Tournament;
            }
            else
            {
                return null;
            }
        }
        public async Task<Result> UpdateTournament(TournamentVM editVM)
        {
            string token = _localStorage.GetStorageValue<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formDataContent = new MultipartFormDataContent();

            formDataContent.Add(new StringContent(editVM.Id), "id");
            formDataContent.Add(new StringContent(editVM.Name), "name");
            formDataContent.Add(new StringContent(editVM.Description), "description");
            formDataContent.Add(new StringContent(editVM.TournamentVideo), "tournamentVideo");

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
        public string GetYouTubeVideoId(string videoLink)
        {
            var uri = new Uri(videoLink);
            var query = HttpUtility.ParseQueryString(uri.Query);
            var videoId = query.Get("v");
            return videoId;
        }

    }
}
