using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Core.Domain;
using Core.EF;
using Core.Infrastructure.EF.DbContext;
using Core.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Infrastructure.EF.Repository;

public abstract class BaseRootRepository{}

public abstract class Repository<TDbContext, TEntity> : BaseRootRepository, IRepository<TEntity>
        where TEntity : BaseEntity
        where TDbContext : DbContext.BaseDbContext
    {
        protected TDbContext DBContext { get; }
        protected readonly DbSet<TEntity> DBSet;

        protected Repository(TDbContext dbContext)
        {
            DBContext = dbContext;
            DBSet = dbContext.Set<TEntity>();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            CancellationToken cancellationToken = default)
        {
            IDbContextTransaction dbContextTransaction = await DBContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return dbContextTransaction;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var entry = await DBSet.AddAsync(entity, cancellationToken);
            if (autoSave)
                await SaveChangeAsync(cancellationToken);
            return entry.Entity;
        }

        public virtual async ValueTask<bool> InsertAsync(IEnumerable<TEntity> entities, bool autoSave, CancellationToken cancellationToken = default)
        {
            DBSet.AddRange(entities);
            if (autoSave)
                return await SaveChangeAsync(cancellationToken) > 0;
            return default;
        }

        public virtual async ValueTask<bool> DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default)
        {

            DBSet.Remove(entity);
            if (autoSave)
                return await SaveChangeAsync(cancellationToken) > 0;
            return default;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var entry = DBSet.Entry(entity);
            entry.State = EntityState.Modified;
            if (autoSave)
                await SaveChangeAsync(cancellationToken);
            return entry.Entity;
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var query = DBSet.AsQueryable();
            if (predicate is not null)
                query = query.Where(predicate);
            return query.LongCountAsync(cancellationToken);
        }

        public virtual Task<bool> ExistAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken = default)
        {
            if (predicate is null)
                return DBSet.AnyAsync(cancellationToken);
            return DBSet.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<PagedResult<TEntity>> GetPagingResultAsync(int pageSize, int pageIndex, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (pageSize == 0)
            {
                pageSize = 50;
            }

            var query = DBSet.AsQueryable();
            long totalCount = await query.CountAsync(cancellationToken);
            IEnumerable<TEntity> items = await query.Where(predicate!)
                                        .ToListAsync(cancellationToken);
            var data = items.Skip(pageIndex * pageSize).Take(pageSize);
            return new(totalCount, data);
        }

        public async Task<PagedResult<TEntity>> GetPagingResultAsync(int pageSize, int pageIndex, IQueryable<TEntity> queryable,
            CancellationToken cancellationToken = default)
        {
            if (pageSize == 0)
            {
                pageSize = 50;
            }

            var items = await queryable.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            long totalCount = await queryable.CountAsync(cancellationToken);
            return new(totalCount, items);
        }

        public Task<IEnumerable<T>> GetFromRawQueryStringAsync<T>(string queryString, object? parameter, CancellationToken cancellationToken = default) where T : notnull
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

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return DBContext.SaveChangesAsync(cancellationToken);
        }

        public void ClearChangeTracker()
        {
            DBContext.ChangeTracker.Clear();
        }
    }