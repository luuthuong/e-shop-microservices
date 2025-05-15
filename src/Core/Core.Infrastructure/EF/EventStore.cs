using System.Text.Json;
using Core.Domain;
using Core.EF;
using Core.Infrastructure.EF.DBContext;
using Core.Infrastructure.Reflections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.EF;

public class EventStore<T>(EventSourcingDbContext dbContext, IServiceScopeFactory serviceScopeFactory)
    : IEventStore<T> where T : AggregateRoot, new()
{
    public async Task SaveAsync(T aggregate)
    {
        var events = aggregate.GetUncommittedEvents();

        if (!events.Any())
            return;

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await SaveEventsAsync(events);
                await dbContext.SaveChangesAsync();
                
                await ProjectionAsync(@events);
                
                await transaction.CommitAsync();
                
                aggregate.ClearUncommittedEvents();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task SaveAsync(IEnumerable<T> aggregateItems)
    {
        var items = aggregateItems.ToList();
        
        if (items.Count == 0)
        {
            return;
        }
        
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var events in items.Select(aggregate => aggregate.GetUncommittedEvents()))
                {
                    await SaveEventsAsync(events);
                }

                await dbContext.SaveChangesAsync();
                
                foreach (var aggregate in items)
                {
                    var events = aggregate.GetUncommittedEvents();
                    await ProjectionAsync(events);
                    aggregate.ClearUncommittedEvents();
                }
                
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<T?> LoadAsync(Guid aggregateId)
    {
        var events = await dbContext.Events
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.Version)
            .AsNoTracking()
            .ToListAsync();

        if (events.Count == 0)
            return null;

        var aggregate = new T();
        var domainEvents = events.Select(e => DeserializeEvent(e.Data, e.Type)!);
        aggregate.LoadFromHistory(domainEvents.ToList());

        return aggregate;
    }

    public async Task<IEnumerable<T>> LoadAsync(IEnumerable<Guid> aggregateIds)
    {
        var events = await dbContext.Events
            .Where(e => aggregateIds.Contains(e.AggregateId))
            .OrderBy(e => e.Version)
            .GroupBy(e => e.AggregateId)
            .AsNoTracking()
            .ToListAsync();
        
        if (events.Count == 0)
            return [];
        
        var aggregates = new List<T>();
        foreach (var group in events)
        {
            var aggregate = new T();
            var domainEvents = group.Select(e => DeserializeEvent(e.Data, e.Type)!);
            aggregate.LoadFromHistory(domainEvents.ToList());
            aggregates.Add(aggregate);
        }
        
        return aggregates;
    }

    private async Task ProjectionAsync(
        DomainEvent @event)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var projections = scope.ServiceProvider.GetServices<IProjection>();

        foreach (var projection in projections)
        {
            if (projection is IStreamProjection streamProjection &&
                streamProjection.HandledEventTypes.Contains(@event.GetType()))
            {
                await projection.ProjectEvent(@event);
            }
        }
    }

    private Task ProjectionAsync(IEnumerable<DomainEvent> @events)
    {   
        var tasks = events.Select(ProjectionAsync);
        return Task.WhenAll(tasks);
    }
    
    public async Task AppendToOutboxAsync(IntegrationEvent @event)
    {
        await dbContext.OutboxEvents.AddAsync(@event);
    }

    public async Task AppendToOutboxAsync(IEnumerable<IntegrationEvent> @event)
    {
        await dbContext.OutboxEvents.AddRangeAsync(@event);
    }

    public async Task AppendToOutboxAsync(DomainEvent @event)
    {
        var integrationEvent = IntegrationEvent.FromDomainEvent(@event);

        await dbContext.OutboxEvents.AddAsync(integrationEvent);
    }

    public async Task AppendToOutboxAsync(IEnumerable<DomainEvent> events)
    {
        var integrationEvents = events.Select(IntegrationEvent.FromDomainEvent);
        await dbContext.OutboxEvents.AddRangeAsync(integrationEvents);
    }

    private async Task SaveEventsAsync(IEnumerable<DomainEvent> events)
    {
        var dataEvents = events.Select(@event => new EventData
        {
            Id = Guid.NewGuid(),
            AggregateId = @event.AggregateId,
            Type = @event.GetType().Name,
            Version = @event.Version,
            Data = JsonSerializer.Serialize(@event, @event.GetType()),
            Timestamp = @event.Timestamp
        });

        await dbContext.Events.AddRangeAsync(dataEvents);
    }

    private static DomainEvent? DeserializeEvent(string data, string type)
    {
        var eventType = TypeGetter.GetTypeFromCurrentDomainAssembly(type);
        ArgumentNullException.ThrowIfNull(eventType);

        return JsonSerializer.Deserialize(data, eventType) as DomainEvent;
    }
}