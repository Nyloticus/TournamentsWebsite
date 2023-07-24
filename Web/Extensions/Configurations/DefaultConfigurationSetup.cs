using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Web.Builders.Interfaces;
using Web.Middlewares;

namespace Web.Extensions.Configurations
{
    public class DefaultConfigurationSetup : IConfigurationSetup
    {
        public void SetupConfiguration(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TournamentWebsite v1"));

            app.UseStaticFiles();

            app.UseRouting();
            // app.UseHttpsRedirection();

            //fast report
            app.UseFastReport();
            app.UseAuthorization();
            app.UseMiddleware<ApiExceptionHandlerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapSwagger();
            });
        }
    }
}