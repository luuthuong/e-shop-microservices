using Core.EF;
using Core.EventBus;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.EF.Repository;

public class EventOutboxRepository(Microsoft.EntityFrameworkCore.DbContext dbContext): IEventOutboxRepository
{
    private readonly DbSet<IntegrationEvent> _dbSet = dbContext.Set<IntegrationEvent>();
    
    public async Task InsertAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(@event, cancellationToken);
    }

    public async Task InsertAsync(IEnumerable<IntegrationEvent> events, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(events, cancellationToken);
    }
}