using Domain.Entities.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlRepositories;

public class SqlTournamentTeamRepository : BaseRepositoryImpl<TournamentTeam>, ITournamentTeamRepository
{
    public SqlTournamentTeamRepository(DbSet<TournamentTeam> table) : base(table)
    {

    }
}