using Domain.Entities.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlRepositories;

public class SqlTournamentRepository : BaseRepositoryImpl<Tournament>, ITournamentRepository
{
    public SqlTournamentRepository(DbSet<Tournament> table) : base(table)
    {

    }
}