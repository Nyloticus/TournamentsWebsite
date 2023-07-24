using Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class FluentApiExtension
    {
        public static IRuleBuilderOptions<T, TProperty> IdNotExist<T, TProperty>(this IRuleBuilder<T, TProperty> rule, IQueryable<BaseEntity<string>> query)
        {
            rule.MustAsync((prop, cancellationToken) =>
            {
                var task = query.AnyAsync(e => e.Id == prop.ToString(), cancellationToken);
                task.Wait(cancellationToken);
                return Task.FromResult(!task.Result);
            });
            return (IRuleBuilderOptions<T, TProperty>)rule;
        }
        public static IRuleBuilderOptions<T, TProperty> MobileNotExist<T, TProperty>(this IRuleBuilder<T, TProperty> rule, IQueryable<BaseEntity<string>> query)
        {
            rule.MustAsync((prop, cancellationToken) =>
            {
                var task = query.AnyAsync(cancellationToken);
                task.Wait(cancellationToken);
                return Task.FromResult(!task.Result);
            });
            return (IRuleBuilderOptions<T, TProperty>)rule;
        }
        public static IRuleBuilderOptions<T, TProperty> EmailNotExist<T, TProperty>(this IRuleBuilder<T, TProperty> rule, IQueryable<BaseEntity<string>> query)
        {
            rule.MustAsync((prop, cancellationToken) =>
            {
                var task = query.AnyAsync(cancellationToken);
                task.Wait(cancellationToken);
                return Task.FromResult(!task.Result);
            });
            return (IRuleBuilderOptions<T, TProperty>)rule;
        }
    }
}