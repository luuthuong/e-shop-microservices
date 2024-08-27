using Core.Domain;
using MediatR;
using Newtonsoft.Json;

namespace Core.Infrastructure.Domain;

public class IntegrationDomainEvent : IIntegrationDomainEvent
{
    public Guid Id => Guid.NewGuid();

    public string EventName { get; }
    public string JsonPayload { get; }

    public static IntegrationDomainEvent Create(INotification notification)
    {
        return new IntegrationDomainEvent(notification);
    }

    public IntegrationDomainEvent()
    {
    }

    private IntegrationDomainEvent(INotification @event)
    {
        EventName = @event.GetType().Name;
        JsonPayload = JsonConvert.SerializeObject(@event);
    }
}