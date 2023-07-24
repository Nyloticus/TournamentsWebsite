#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IHttpClient
    {
        HttpRequestHeaders DefaultRequestHeaders { get; }
        Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);
    }
    public class MainHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public MainHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpRequestHeaders DefaultRequestHeaders => _httpClient.DefaultRequestHeaders;

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
        {
            return _httpClient.GetAsync(requestUri, completionOption);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            return _httpClient.SendAsync(request, completionOption);
        }
        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = requestUri,
                Content = content,
            };

            return _httpClient.SendAsync(httpRequestMessage);
        }



        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            return _httpClient.PostAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return _httpClient.DeleteAsync(requestUri);
        }
    }

    public abstract class RestApiClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _baseEndPoint;
        private readonly JsonSerializerSettings? _jsonSettings;
        protected RestApiClient(IHttpClient httpClient, string baseEndPoint)
        {
            _httpClient = httpClient;
            _baseEndPoint = baseEndPoint;
            _jsonSettings = ConfigureJsonSettings();
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            BaseEndPoint = new Uri(_baseEndPoint);
            // _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
        }

        private Uri BaseEndPoint { get; set; }
        private JsonSerializerSettings? ConfigureJsonSettings()
        {
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsonSettings.Converters.Add(new StringEnumConverter
            {
                NamingStrategy = new DefaultNamingStrategy { }
            });

            return jsonSettings;
        }

        public async Task<TResponse?> GetAsync<TResponse>(string requestUrl)
        {
            var fullUrl = new Uri(BaseEndPoint, requestUrl);

            var response = await _httpClient.GetAsync(fullUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(data, _jsonSettings);
        }

        public async Task<TResponse?> DeleteAsync<TResponse>(string requestUrl)
        {
            var fullUrl = new Uri(BaseEndPoint, requestUrl);

            var response = await _httpClient.DeleteAsync(fullUrl);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(data, _jsonSettings);
        }

        public async Task<TResponse?> PostAsync<TResponse>(string requestUrl, object request)
        {
            try
            {
                var fullUrl = new Uri(BaseEndPoint, requestUrl);
                var response = await _httpClient.PostAsync(fullUrl, CreateHttpContent(request));
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResponse>(data, _jsonSettings);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<TResponse?> PostAsync<TResponse, TRequest>(string requestUrl, TRequest request)
        {
            try
            {
                var fullUrl = new Uri(BaseEndPoint, requestUrl);
                var response = await _httpClient.PostAsync(fullUrl, CreateHttpContent<TRequest>(request));
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResponse>(data, _jsonSettings);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, _jsonSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }

    //public class FileServiceHttpClient : RestApiClient
    //{
    //    public FileServiceHttpClient([NotNull] IHttpClient httpClient, [NotNull] IOptions<ServicesUrlOption> baseEndPoint) : base(httpClient, baseEndPoint.Value.FileService) { }

    //    public async Task<TResponse> UploadFile<TResponse, TRequest>(TRequest fileDto)
    //    {
    //        return await this.PostAsync<TResponse>("file/upload/", fileDto);
    //    }

    //    public async Task<TResponse> RemoveFile<TResponse>(string url)
    //    {
    //        return await this.DeleteAsync<TResponse>("file/remove/" + url);
    //    }
    //}
}