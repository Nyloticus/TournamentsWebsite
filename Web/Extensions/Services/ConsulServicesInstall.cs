using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Web.Builders.Interfaces;

namespace Web.Extensions.Services
{
    public class ConsulServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration.GetValue<string>("ConsulOptions:Host");
                consulConfig.Address = new Uri(address);
                Console.WriteLine(address);
            }));
        }
    }
}