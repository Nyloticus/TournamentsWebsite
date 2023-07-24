using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Web.Builders.Interfaces;

namespace Web.Extensions.Configurations
{
    public class MiddlewaresConfigurationSetup : IConfigurationSetup
    {
        public void SetupConfiguration(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseMiddleware<ApiExceptionHandlerMiddleware>();
        }
    }
}