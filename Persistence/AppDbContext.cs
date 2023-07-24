using Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Entities.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class AppDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>,
        UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IAppDbContext
    {
        private readonly IAuditService _auditService;

        #region Dbsets

        public DbSet<Setting> Settings { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<OTPVerification> OTPVerifications { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<UserImages> UserImages { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TournamentTeam> TournamentTeams { get; set; }
        public DbSet<Player> Players { get; set; }


        #endregion


        public AppDbContext(DbContextOptions options, IAuditService auditService = null)
        : base(options)
        {
            _auditService = auditService;

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(builder);
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(256));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(256));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderDisplayName).HasMaxLength(256));

            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(256));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(256));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(256));

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.Property(p => p.RoleId).HasMaxLength(256);
                userRole.Property(p => p.UserId).HasMaxLength(256);

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            Database.CommitTransaction();
        }

        public void Rollback()
        {
            Database.RollbackTransaction();
        }



        #region overrides

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if (entity is ICreateAudit audit)
            {
                audit.CreatedBy = _auditService.UserId;
                audit.CreatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = _auditService.TenantId;
            return base.Add(entity);
        }
        public override EntityEntry Add(object entity)
        {
            if (entity is ICreateAudit audit)
            {
                audit.CreatedBy = _auditService.UserId;
                audit.CreatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = _auditService.TenantId;

            return base.Add(entity);
        }
        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (entity is ICreateAudit audit)
            {
                audit.CreatedBy = _auditService.UserId;
                audit.CreatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = _auditService.TenantId;

            return base.AddAsync(entity, cancellationToken);
        }
        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity is ICreateAudit audit)
            {
                audit.CreatedBy = _auditService.UserId;
                audit.CreatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = _auditService.TenantId;

            return base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry Update(object entity)
        {
            if (entity is IUpdateAudit audit)
            {
                audit.UpdatedBy = _auditService.UserId;
                audit.UpdatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = _auditService.TenantId;
            return base.Update(entity);
        }
        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            if (entity is IUpdateAudit audit)
            {
                audit.UpdatedBy = _auditService.UserId;
                audit.UpdatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = _auditService.TenantId;

            return base.Update(entity);
        }

        public override EntityEntry Remove(object entity)
        {
            if (entity is IDeleteEntity audit)
            {
                audit.DeletedBy = _auditService.UserId;
                audit.DeletedDate = DateTime.UtcNow;
                audit.IsDeleted = true;
                return Update(entity);
            }
            return base.Remove(entity);
        }
        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            if (entity is IDeleteEntity audit)
            {
                audit.DeletedBy = _auditService.UserId;
                audit.DeletedDate = DateTime.UtcNow;
                audit.IsDeleted = true;
                return Update(entity);
            }
            return base.Remove(entity);
        }

        #endregion

        public async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(CancellationToken.None);
        }
    }

}
