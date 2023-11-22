using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Core.Domain;
using Core.Results;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.EF;

 public interface IRepository <TEntity>
        where TEntity: BaseEntity
    {
        Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            CancellationToken cancellationToken = default);
         Task<TEntity> InsertAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);

         ValueTask<bool> InsertAsync(IEnumerable<TEntity> entities, bool autoSave,
             CancellationToken cancellationToken = default);
         ValueTask<bool> DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);
         Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);
         Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,CancellationToken cancellationToken = default);
         Task<bool> ExistAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken = default);
         Task<PagedResult<TEntity>> GetPagingResultAsync(int pageSize, int pageIndex, Expression<Func<TEntity, bool>> predicate,
             CancellationToken cancellationToken = default);
         Task<PagedResult<TEntity>> GetPagingResultAsync(int pageSize, int pageIndex, IQueryable<TEntity> queryable,
             CancellationToken cancellationToken = default);
         Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, object? parameter, CancellationToken cancellationToken = default) where T: notnull;
         Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, IEnumerable<object> parameter, CancellationToken cancellationToken = default) where T: notnull;
         Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, IEnumerable<DbParameter> parameter, CancellationToken cancellationToken = default) where T: notnull;

         Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
         void ClearChangeTracker();
    }