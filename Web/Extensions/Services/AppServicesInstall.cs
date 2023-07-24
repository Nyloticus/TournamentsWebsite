using Application.Extensions;
using Common.Interfaces;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Web.Builders.Interfaces;

namespace Web.Extensions.Services
{
    public class AppServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection
                      .AddScoped<IPermissionService, PermissionService>()
              .AddScoped<IAppDbContext>(s => s.GetService<AppDbContext>()!)
              .AddScoped<IAuditService, AuditService>()
              .AddScoped<IIdentityService, IdentityService>()
              .AddScoped<IUrlHelper, UrlHelper>()
              .AddScoped<IHttpClient, MainHttpClient>()
              .AddHttpClient()
              .AddScoped<AppUserManager>()
              .AddSingleton<ISettingService, SettingService>();

        }
    }
}