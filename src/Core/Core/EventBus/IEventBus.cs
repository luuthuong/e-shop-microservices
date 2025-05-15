using Core.Domain;

namespace Core.EventBus;

public interface IEventBus
{
    Task PublishAsync(IIntegrationEvent @event);
    Task PublishAsync(IEnumerable<IIntegrationEvent> events);
}