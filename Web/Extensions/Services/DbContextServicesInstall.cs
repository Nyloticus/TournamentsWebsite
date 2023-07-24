using Common.Options;
using Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.MSSQL;
//using Persistence.MYSQL;
using Web.Builders.Interfaces;
using Web.Globals;

namespace Web.Extensions.Services
{
    public class DbContextServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var persistenceConfig = configuration?.GetSection(nameof(Sections.Persistence))?.Get<PersistenceConfiguration>();

            if (persistenceConfig?.Provider == "MSSQL")
            {
                services.AddMssqlDbContext(configuration);
                //services.AddDbContext<AppDbContext, MsSqlAppDbContext>();
            }

            //if (persistenceConfig?.Provider == "MYSQL") {
            //  //services.AddMySqlDbContext(configuration);
            //  services.AddMysqlDbContext(configuration);
            //}

            services
              .AddIdentity<User, Role>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
        }
    }
}