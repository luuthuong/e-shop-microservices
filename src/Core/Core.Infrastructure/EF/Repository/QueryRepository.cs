using System.Data.Common;
using System.Linq.Expressions;
using Core.EF;
using Core.Infrastructure.EF.DBContext;
using Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.EF.Repository;

public class QueryRepository<TEntity, TContext>(TContext context) : IQueryRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<TEntity?> FindByAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public Task<bool> ExistAsync(Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default) =>
        predicate == null ? _dbSet.AnyAsync(cancellationToken) : _dbSet.AnyAsync(predicate, cancellationToken);

    public Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, object? parameter,
        CancellationToken cancellationToken = default) where T : notnull
    {
        var parameters = new List<object>();
        if (parameter is not null)
        {
            parameters.Add(parameter);
        }

        return context.GetFromQueryAsync<T>(queryString, parameters, cancellationToken);
    }

    public Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, IEnumerable<object> parameters,
        CancellationToken cancellationToken = default) where T : notnull
    {
        return context.GetFromQueryAsync<T>(queryString, parameters, cancellationToken);
    }

    public Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, IEnumerable<DbParameter> parameters,
        CancellationToken cancellationToken = default) where T : notnull
    {
        return context.GetFromQueryAsync<T>(queryString, parameters, cancellationToken);
    }

    public async Task<PagedResult<TEntity>> GetPagingResultAsync(int pageSize, int pageIndex,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        if (pageSize == 0)
            pageSize = 50;

        var query = _dbSet.AsQueryable();
        long totalCount = await query.CountAsync(cancellationToken);
        IEnumerable<TEntity> items = await query.Where(predicate!)
            .ToListAsync(cancellationToken);
        var data = items.Skip(pageIndex * pageSize).Take(pageSize);

        return new(totalCount, pageSize, pageIndex, data);
    }

    public async Task<PagedResult<TEntity>> GetPagingResultAsync(int pageSize, int pageIndex,
        IQueryable<TEntity> queryable,
        CancellationToken cancellationToken = default)
    {
        if (pageSize == 0)
        {
            pageSize = 50;
        }

        var items = await queryable.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        long totalCount = await queryable.CountAsync(cancellationToken);
        return new(totalCount, pageSize, pageIndex, items);
    }
}