using MVC.Models;
using MVC.Services.Base;
using System.Text.Json;

namespace MVC.Services
{
    public static class HttpClientExtension
    {
        public static async Task<List<T>> ReadContentListAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(dataAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return apiResponse.Payload;

        }
        public static async Task<T> ReadContentAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var apiResponse = JsonSerializer.Deserialize<Result>(dataAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var payload = JsonSerializer.Deserialize<T>(apiResponse.Payload.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return payload;
        }
        public static async Task<Result> ReadFromResultAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var apiResponse = JsonSerializer.Deserialize<Result>(dataAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return apiResponse;

        }
    }
}
