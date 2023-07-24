using Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories;

public interface IBaseRepository<T> where T : BaseEntity<string>
{
    Task<Result<T>> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties);
    Task<Result<T[]>> GetByMultipleIdsAsync(List<string> id, params Expression<Func<T, object>>[] includeProperties);
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T[]>> AddRangeAsync(T[] entities);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result<T[]>> UpdateRangeAsync(T[] entities);
    Task<Result<T>> DeleteAsync(T entity);
    Task<Result<T[]>> DeleteRangeAsync(T[] entities);
    Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

    Task<List<T>> GetAllByAsync(Expression<Func<T, bool>> predict,
        params Expression<Func<T, object>>[] includeProperties);

}