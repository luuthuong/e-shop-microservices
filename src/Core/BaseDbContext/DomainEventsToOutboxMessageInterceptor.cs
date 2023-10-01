using Core.BaseDomain;
using Core.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Core.BaseDbContext;

public class DomainEventsToOutboxMessageInterceptor: SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
        )
    {
        DbContext? context = eventData.Context;
        if(context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        var outboxMessages = context.ChangeTracker.Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                var @events = x.GetDomainEvents();
                x.ClearDomainEvents();
                return @events;
            })
            .Select(x => new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                ExecutedOnUtc = DateTime.Now,
                Type = x.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    x, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            }).ToList();
        context.Set<OutboxMessage>().AddRange(outboxMessages);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}