using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Core.Domain;

public record class IntegrationEvent : IIntegrationEvent
{
    private IntegrationEvent(DomainEvent @event)
    {
        Id = Guid.NewGuid();
        EventName = @event.GetType().Name;
        JsonPayload = SerializeObject(@event);
    }

    public IntegrationEvent()
    {
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [MaxLength(200)] public string EventName { get; private set; }

    public string? JsonPayload { get; private set; } = string.Empty;

    public static IntegrationEvent FromDomainEvent(DomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return new IntegrationEvent(@event);
    }

    public static IntegrationEvent Create(object payload, string? eventName = null)
    {
        return new IntegrationEvent
        {
            EventName = eventName ?? payload.GetType().Name,
            JsonPayload = SerializeObject(payload)
        };
    }

    private static string SerializeObject(object value)
    {
        return JsonConvert.SerializeObject(
            value,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            }
        );
    }
}