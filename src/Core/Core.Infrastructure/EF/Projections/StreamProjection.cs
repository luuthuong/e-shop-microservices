using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.EF.Projections;

public abstract class StreamProjection<T, TContext>(
    TContext dbContext
)
    : AbstractStreamProjection<T>
    where T : class, new()
    where TContext : DbContext
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public override async Task ProjectEvent(DomainEvent @event, CancellationToken cancellationToken = default)
    {
        if (!Exists(@event))
            return;
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var item = await LoadOrCreateItemAsync(@event.AggregateId, cancellationToken);
                var handler = GetHandler(@event);
                item = handler(item, @event);
                await SaveItemAsync(item, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (System.Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    protected override async Task<T> LoadOrCreateItemAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _dbSet.FindAsync(id, cancellationToken);
        
        if (item is null)
            return new T();
        
        dbContext.Entry(item).State = EntityState.Unchanged;
        return item;
    }

    protected override async Task SaveItemAsync(T item, CancellationToken cancellationToken = default)
    {
        var entry = _dbSet.Entry(item);

        if (entry.State == EntityState.Detached)
            await dbContext.AddAsync(item, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}