using Common;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Persistence.SqlRepositories;

public class BaseRepositoryImpl<T> : IBaseRepository<T> where T : BaseEntity<string>
{
    protected readonly DbSet<T> table;

    public BaseRepositoryImpl(DbSet<T> table)
    {
        this.table = table;
    }

    public async Task<Result<T>> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties)
    {
        try
        {
            var query = table.AsQueryable();
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            var result = await query.FirstOrDefaultAsync(x => x.Id == id);
            return result == null
                ? Result<T>.Failure(ApiExeptionType.NotFound, "Not found")
                : Result<T>.Successed(result);
        }
        catch (Exception e)
        {
            return Result<T>.Failure(ApiExeptionType.FailedGetData, e.Message);
        }
    }
    public async Task<Result<T[]>> GetByMultipleIdsAsync(List<string> id, params Expression<Func<T, object>>[] includeProperties)
    {
        try
        {
            var query = table.AsQueryable();
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            var result = await query.Where(x => id.Contains(x.Id)).ToArrayAsync();
            if (result.Length == 0)
                return Result<T[]>.Failure(ApiExeptionType.NotFound, "Not found");
            //if (result.Length != id.Count)
            //    return Result<T[]>.Failure(ApiExeptionType.NotFound, $"These ids are not found{string.Join(",", id.Except(result.Select(x => x.Id)))}");
            return Result<T[]>.Successed(result);
        }
        catch (Exception e)
        {
            return Result<T[]>.Failure(ApiExeptionType.FailedGetData, e.Message);
        }
    }

    public Task<Result<T[]>> AddRangeAsync(T[] entities)
    {
        try
        {
            table.AddRangeAsync(entities);
            return Task.FromResult(Result<T[]>.Successed(entities));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromResult(Result<T[]>.Failure(ApiExeptionType.FailedSaveData, e.Message));
        }
    }

    public Task<Result<T[]>> UpdateRangeAsync(T[] entities)
    {
        try
        {
            table.UpdateRange(entities);
            return Task.FromResult(Result<T[]>.Successed(entities));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromResult(Result<T[]>.Failure(ApiExeptionType.FailedUpdateData, e.Message));
        }
    }

    public Task<Result<T[]>> DeleteRangeAsync(T[] entities)
    {
        try
        {
            table.RemoveRange(entities);
            return Task.FromResult(Result<T[]>.Successed(entities));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromResult(Result<T[]>.Failure(ApiExeptionType.FailedDeleteData, e.Message));
        }
    }

    public async Task<Result<T>> AddAsync(T entity)
    {
        try
        {
            await table.AddAsync(entity);
            return Result<T>.Successed(entity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<T>.Failure(ApiExeptionType.FailedSaveData, e.Message);
        }
    }

    public async Task<Result<T>> UpdateAsync(T entity)
    {
        try
        {
            table.Update(entity);
            return Result<T>.Successed(entity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<T>.Failure(ApiExeptionType.FailedUpdateData, e.Message);
        }
    }

    public async Task<Result<T>> DeleteAsync(T entity)
    {
        try
        {
            table.Remove(entity);
            return Result<T>.Successed(entity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<T>.Failure(ApiExeptionType.FailedDeleteData, e.Message);
        }
    }

    public Task<List<T>> GetAllByAsync(Expression<Func<T, bool>> predict,
        params Expression<Func<T, object>>[] includeProperties)
    {
        try
        {
            var query = table.Where(predict);
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            return query.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromResult(new List<T>());
        }
    }


    public Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        try
        {
            var query = table.AsQueryable();
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            return query.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromResult(new List<T>());
        }
    }
}