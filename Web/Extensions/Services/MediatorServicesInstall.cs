using Common.Infrastructures.MediatR;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Web.Builders.Interfaces;
using Web.Middlewares;

namespace Web.Extensions.Services
{
    public class MediatorServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddMediatR(new[] {
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(Application.Team.Commands.CreateTeamCommand)),
      });
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(DbContextTransactionPipeline<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}