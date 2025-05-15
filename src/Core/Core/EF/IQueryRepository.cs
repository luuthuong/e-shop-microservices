using System.Data.Common;
using System.Linq.Expressions;
using Core.Results;

namespace Core.EF;

public interface IQueryRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, object? parameter,
        CancellationToken cancellationToken = default) where T : notnull;

    Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, IEnumerable<object> parameter,
        CancellationToken cancellationToken = default) where T : notnull;

    Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, IEnumerable<DbParameter> parameter,
        CancellationToken cancellationToken = default) where T : notnull;

    Task<PagedResult<TEntity>> GetPagingResultAsync(int pageSize, int pageIndex,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TEntity>> GetPagingResultAsync(int pageSize, int pageIndex, IQueryable<TEntity> queryable,
        CancellationToken cancellationToken = default);
}