using Common.Infrastructures;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> Protected<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            var result = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
            if (result)
            {
                source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
            }
            source = source.Where(predicate);
            return source;
        }

        public static IQueryable<TSource> Protected<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            var result = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
            if (result)
            {
                source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
            }
            source = source.Where(predicate);
            return source;
        }
        public static IQueryable<TSource> Protected<TSource>(this IQueryable<TSource> source)
        {
            var result = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
            if (result)
            {
                source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
            }

            return source;
        }


        public static IEnumerable<TSource> WhereProtected<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            var result = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
            if (result)
            {
                source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
            }
            source = source.Where(predicate);
            return source;
        }

        public static IEnumerable<TSource> WhereProtected<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var result = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
            if (result)
            {
                source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
            }
            source = source.Where(predicate);
            return source;
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, Paging sort)
        {
            if (String.IsNullOrEmpty(sort.SortBy))
                return source;



            var sortFn = sort.SortOrder == "asc" ? "OrderBy" : "OrderByDescending";
            var type = typeof(T);
            var p = Expression.Parameter(type, "p");
            var propertyAccess = GetProperty(p, type, sort.SortBy);
            var orderByExpression = Expression.Lambda(propertyAccess, p);
            var resultExpression = Expression.Call(typeof(Queryable), sortFn, new Type[] { type, propertyAccess.Type },
              source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        public static MemberExpression GetProperty(Expression p, Type type, string propPath)
        {
            if (propPath == null)
                return null;
            var propParts = propPath.Split('.');
            while (propParts.Count() > 1)
            {
                var properName = propParts[0];
                var prop = GetProperty(p, type, properName);
                var _p = Expression.MakeMemberAccess(p, prop.Member);
                return GetProperty(_p, prop.Type, string.Join(".", propParts.Skip(1)));
            }



            if (!propPath.Contains("."))
            {
                var property = type.GetProperty(propPath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                return Expression.MakeMemberAccess(p, property);
            }



            return null;
        }
    }
}