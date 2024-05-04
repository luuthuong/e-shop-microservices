using Core.Domain;
using Core.EF;
using Core.Infrastructure.EF.DbContext;
using Core.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace Core.Infrastructure.EF.Repository;

public abstract class BaseRootRepository
{
}

public abstract class Repository<TDbContext, TEntity, TId>(TDbContext dbContext) : BaseRootRepository, IRepository<TEntity, TId>
    where TId: StronglyTypeId<Guid>
    where TEntity : BaseEntity<TId>
    where TDbContext : BaseDbContext
{
    private TDbContext DBContext { get; } = dbContext;
    protected readonly DbSet<TEntity> DBSet = dbContext.Set<TEntity>();

    public virtual Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return DBSet.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await DBSet.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public virtual ValueTask<bool> InsertAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        DBSet.AddRange(entities);
        return default;
    }

    public virtual async ValueTask<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DBSet.Remove(entity);
        return await Task.FromResult(true);
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = DBSet.Entry(entity);
        entry.State = EntityState.Modified;
        return entry.Entity;
    }

    public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = DBSet.AsQueryable();
        if (predicate is not null)
            query = query.Where(predicate);
        return query.LongCountAsync(cancellationToken);
    }

    public virtual Task<bool> ExistAsync(Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        if (predicate is null)
            return DBSet.AnyAsync(cancellationToken);
        return DBSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<PagedResult<TEntity>> GetPagingResultAsync(
        int pageSize,
        int pageIndex,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        if (pageSize == 0)
            pageSize = 50;

        var query = DBSet.AsQueryable();
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

    public Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, object? parameter,
        CancellationToken cancellationToken = default) where T : notnull
    {
        var parameters = new List<object>();
        if (parameter is not null)
        {
            parameters.Add(parameter);
        }

        return DBContext.GetFromQueryAsync<T>(queryString, parameters, cancellationToken);
    }

    public Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, IEnumerable<object> parameters,
        CancellationToken cancellationToken = default) where T : notnull =>
        DBContext.GetFromQueryAsync<T>(queryString, parameters, cancellationToken);

    public Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, IEnumerable<DbParameter> parameters,
        CancellationToken cancellationToken = default) where T : notnull =>
        DBContext.GetFromQueryAsync<T>(queryString, parameters, cancellationToken);
}