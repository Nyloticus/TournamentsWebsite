using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<T> Set<T>() where T : class;

        DbSet<Setting> Settings { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserImages> UserImages { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<OTPVerification> OTPVerifications { get; set; }
        DbSet<UserDevice> UserDevices { get; set; }
        DbSet<Tournament> Tournaments { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<TournamentTeam> TournamentTeams { get; set; }
        DbSet<Player> Players { get; set; }

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> CreateTransaction();

        void Commit();
        void Rollback();
    }
}