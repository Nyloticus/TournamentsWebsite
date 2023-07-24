using Common.Options;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Web.Builders.Interfaces;

namespace Web.Extensions.Configurations
{
    public class ConsulConfigurationSetup : IConfigurationSetup
    {
        public void SetupConfiguration(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var options = app.ApplicationServices.GetRequiredService<IOptions<ConsulOptions>>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            var serviceName = options.Value.ServiceName;
            var serviceId = $"{serviceName}-{Guid.NewGuid().ToString()}";
            var registration = new AgentServiceRegistration()
            {
                ID = serviceId, //{uri.Port}",
                Name = serviceName,
                Address = options.Value.Address, //$"{uri.Host}",
                Port = options.Value.Port, // uri.Port
            };

            logger.LogInformation("Trying register to Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            });
            logger.LogInformation("Registering with Consul successfully");
        }
    }
}