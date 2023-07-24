using Common;
using Common.Interfaces;
using Domain.Repositories;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class TenantQueryExtension
    {
        private static IServiceScopeFactory _serviceProvider;

        public static void Initialize(IServiceScopeFactory serviceScope)
        {
            _serviceProvider = serviceScope;
        }

        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source)
        {
            // var result = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
            using var scope = _serviceProvider.CreateScope();
            var audit = scope.ServiceProvider.GetService<IAuditService>();
            // if (result) {
            //   if (audit.IsTenantUser)
            //     source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
            // }

            var result = typeof(ITenant).IsAssignableFrom(typeof(TSource));
            if (result)
            {
                if (audit is { IsTenantUser: true })
                    source = source.Where(s => ((ITenant)s).TenantId == audit.TenantId);
            }

            return source;
        }

        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            // var result = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
            using var scope = _serviceProvider.CreateScope();
            var audit = scope.ServiceProvider.GetService<IAuditService>();
            // if (result) {
            //   source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
            // }
            var result = typeof(ITenant).IsAssignableFrom(typeof(TSource));
            if (result)
            {
                if (audit is { IsTenantUser: true })
                    source = source.Where(s => ((ITenant)s).TenantId == audit.TenantId);
            }

            source = source.Where(predicate);
            return source;
        }

        public static void UpdateEntityAudit<T>(this T entity)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetService<IAuditService>();
                if (entity is not IUpdateAudit audit) return;
                audit.UpdatedBy = auditService.UserName;
                audit.UpdatedDate = DateTime.UtcNow;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void AddEntityAudit<T>(this T entity)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetService<IAuditService>();
                if (entity is not ICreateAudit audit) return;
                audit.CreatedBy = auditService.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async ValueTask<EntityEntry<TEntity>> CreateAsync<TEntity>(this DbSet<TEntity> source,
            TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var scope = _serviceProvider.CreateScope();
            var auditService = scope.ServiceProvider.GetService<IAuditService>();

            if (entity is ICreateAudit audit)
            {
                audit.CreatedBy = auditService?.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = auditService?.TenantId;
            return await source.AddAsync(entity, cancellationToken);
        }

        public static async Task<Result<TEntity>> CreateAsync<TEntity>(this IBaseRepository<TEntity> source,
            TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity<string>
        {
            using var scope = _serviceProvider.CreateScope();
            var auditService = scope.ServiceProvider.GetService<IAuditService>();

            if (entity is ICreateAudit audit)
            {
                audit.CreatedBy = auditService?.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = auditService?.TenantId;
            return await source.AddAsync(entity);
        }

        public static async Task<Result<TExternal>> CreateAsyncWith<TEntity, TExternal>(
            this IBaseRepository<TEntity> source,
            TExternal entity,
            Func<TExternal, Task<Result<TExternal>>> perform, CancellationToken cancellationToken = default)
            where TEntity : BaseEntity<string> where TExternal : BaseEntity<string>
        {
            using var scope = _serviceProvider.CreateScope();
            var auditService = scope.ServiceProvider.GetService<IAuditService>();

            if (entity is ICreateAudit audit)
            {
                audit.CreatedBy = auditService?.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }

            if (entity is ITenant tenant)
                tenant.TenantId = auditService?.TenantId;
            return await perform(entity);
        }

        public static EntityEntry<TEntity> Edit<TEntity>(this DbSet<TEntity> source, TEntity entity)
            where TEntity : class
        {
            using var scope = _serviceProvider.CreateScope();
            var auditService = scope.ServiceProvider.GetService<IAuditService>();

            if (entity is IUpdateAudit audit)
            {
                audit.UpdatedBy = auditService?.UserName;
                audit.UpdatedDate = DateTime.UtcNow;
            }

            return source.Update(entity);
        }


        public static Task<Result<TEntity>> Edit<TEntity>(this IBaseRepository<TEntity> source, TEntity entity)
            where TEntity : BaseEntity<string>
        {
            using var scope = _serviceProvider.CreateScope();
            var auditService = scope.ServiceProvider.GetService<IAuditService>();

            if (entity is IUpdateAudit audit)
            {
                audit.UpdatedBy = auditService?.UserName;
                audit.UpdatedDate = DateTime.UtcNow;
            }

            return source.UpdateAsync(entity);
        }

        public static async Task<Result<TExternal>> EditAsyncWith<TEntity, TExternal>(
            this IBaseRepository<TEntity> source,
            TExternal entity,
            Func<TExternal, Task<Result<TExternal>>> perform, CancellationToken cancellationToken = default)
            where TEntity : BaseEntity<string> where TExternal : BaseEntity<string>
        {
            using var scope = _serviceProvider.CreateScope();
            var auditService = scope.ServiceProvider.GetService<IAuditService>();

            if (entity is IUpdateAudit audit)
            {
                audit.UpdatedBy = auditService?.UserName;
                audit.UpdatedDate = DateTime.UtcNow;
            }

            return await perform(entity);
        }
    }

    public static class DbSetExtension
    {
    }
}