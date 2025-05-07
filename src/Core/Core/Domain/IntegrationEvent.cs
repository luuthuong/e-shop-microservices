using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Core.Domain;

public class IntegrationEvent : IIntegrationEvent
{
    private IntegrationEvent(DomainEvent @event)
    {
        Id = Guid.NewGuid();
        EventName = @event.GetType().Name;
        JsonPayload = JsonConvert.SerializeObject(
            @event,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            }
        );
    }

    public IntegrationEvent()
    {
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [MaxLength(200)] 
    public string EventName { get; private set; }

    public string? JsonPayload { get; private set; } = string.Empty;

    public static IntegrationEvent FromDomainEvent(DomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return new IntegrationEvent(@event);
    }
}