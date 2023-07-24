using Mapster;
using MVC.Contracts;
using MVC.Services.Base;
using System.Reflection;

namespace MVC.Services
{
    public static class ServiceExtensions
    {
        public static void AddMyServices(this IServiceCollection services, IConfiguration configuration)
        {
            string baseUrl = configuration.GetValue<string>("BaseUrl");

            services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri(baseUrl));
            services.AddSingleton<ILocalStorageService, LocalStorageService>();
            services.AddHttpClient<ITeamService, TeamService>(cl => cl.BaseAddress = new Uri(baseUrl));
            services.AddHttpClient<ITournamentService, TournamentService>(cl => cl.BaseAddress = new Uri(baseUrl));
            services.AddHttpClient<ITournamentTeamService, TournamentTeamService>(cl => cl.BaseAddress = new Uri(baseUrl));
            services.AddHttpClient<IAuthService, AuthService>(cl => cl.BaseAddress = new Uri(baseUrl));
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }

    }
}
