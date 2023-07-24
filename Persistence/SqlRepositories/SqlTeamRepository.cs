using Domain.Entities.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlRepositories;

public class SqlTeamRepository : BaseRepositoryImpl<Team>, ITeamRepository
{
    public SqlTeamRepository(DbSet<Team> table) : base(table)
    {

    }
}