using System.Collections.Concurrent;
using Core.Domain;
using Core.EF;
using Core.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.EventBus;

public class EventBus(IServiceScopeFactory serviceScopeFactory, ILogger<EventBus> logger) : IEventBus
{
    public async Task PublishAsync(IIntegrationEvent @event)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            logger.LogInformation($"EventBus Handling event: {@event.GetType().Name}");
            var method = handlerType.GetMethod("HandleAsync")!;
            await (Task)method.Invoke(handler, [@event])!;
            logger.LogInformation("EventBus {EventName} Handled!", @event.GetType().Name);
        }
    }

    public async Task PublishAsync(IEnumerable<IIntegrationEvent> events)
    {
        foreach (var @event in events)
        {
            dynamic dynamicEvent = @event;
            await PublishAsync(dynamicEvent);
        }
    }
}