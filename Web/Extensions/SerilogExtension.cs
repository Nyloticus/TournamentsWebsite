using Microsoft.AspNetCore.Builder;

namespace Web.Extensions
{


    public static class SerilogExtension
    {

        public static IApplicationBuilder ConfigSerilog(this IApplicationBuilder builder)
        {
            // var env=builder.Environment.EnvironmentName;
            // var config = new ConfigurationBuilder()
            //   .AddJsonFile("appsettings.json")
            //   .AddJsonFile($"appsettings.{env}.json")
            //   .Build();
            //
            // builder.Host.UseSerilog();
            // Log.Logger = new LoggerConfiguration()
            //   .ReadFrom.Configuration(config)
            //   .Enrich.FromLogContext()
            //   .Enrich.WithProperty("ApplicationName", "Middleware.AuthService")
            //   .CreateLogger();

            return builder;
        }

    }
}
