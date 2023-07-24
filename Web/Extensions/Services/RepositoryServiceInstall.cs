using Domain.Repositories;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Persistence.SqlRepositories;

namespace Web.Extensions.Services;

public static class RepositoryServiceInstall
{
    public static void InstallRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITeamRepository, SqlTeamRepository>(service =>
        {
            var context = service.GetService<IAppDbContext>();
            return new SqlTeamRepository(context.Teams);
        });
        services.AddScoped<ITournamentRepository, SqlTournamentRepository>(service =>
        {
            var context = service.GetService<IAppDbContext>();
            return new SqlTournamentRepository(context.Tournaments);
        });
        services.AddScoped<ITournamentTeamRepository, SqlTournamentTeamRepository>(service =>
        {
            var context = service.GetService<IAppDbContext>();
            return new SqlTournamentTeamRepository(context.TournamentTeams);
        });

    }
}