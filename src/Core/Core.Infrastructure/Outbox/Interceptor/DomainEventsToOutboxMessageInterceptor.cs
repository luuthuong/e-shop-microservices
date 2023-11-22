using Core.Domain;
using Core.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Infrastructure.Outbox.Interceptor;

public class DomainEventsToOutboxMessageInterceptor: SaveChangesInterceptor
{
    private readonly ILogger<DomainEventsToOutboxMessageInterceptor> _logger;

    public DomainEventsToOutboxMessageInterceptor(ILogger<DomainEventsToOutboxMessageInterceptor> logger)
    {
        _logger = logger;
    }

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
            .Select(x =>
            {
                var @event = new OutboxMessage()
                {
                    Id = Guid.NewGuid(),
                    ExecutedOnUtc = DateTime.Now,
                    Type = x.GetType().Name,
                    Content = JsonConvert.SerializeObject(
                        x, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })
                };
                _logger.LogInformation("Adding domain event: {@event}", @event);
                return @event;
            }).ToList();
        context.Set<OutboxMessage>().AddRange(outboxMessages);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}