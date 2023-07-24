using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Builders.Interfaces;

namespace Web.Extensions.Services
{
    public class DefaultServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
              // .AddEndpointsApiExplorer()
              .AddHttpContextAccessor()
              .AddHttpClient()
              .AddControllers()
              ;
        }
    }
}