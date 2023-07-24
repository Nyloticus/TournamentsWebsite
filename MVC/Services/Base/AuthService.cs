using MVC.Contracts;
using MVC.Models;
using System.Text.Json;

namespace MVC.Services.Base
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public const string BasePath = "https://localhost:5001/api/Auth/";

        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        }

        public async Task<bool> Auth(AuthVM authData)
        {
            var response = await _httpClient.PostAsJsonAsync(BasePath + "login", authData);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var loginResponse = await response.ReadFromResultAsync<Result>();

            string token = string.Empty;
            var payload = (JsonElement)loginResponse.Payload;
            if (payload.TryGetProperty("token", out var tokenProperty))
            {
                token = tokenProperty.GetString();
            }

            _localStorageService.SetStorageValue("token", token);
            return true;
        }
    }

}
