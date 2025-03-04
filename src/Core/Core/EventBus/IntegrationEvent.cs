using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Core.Domain;
using MediatR;

namespace Core.EventBus;

public class IntegrationEvent: INotification
{
    private IntegrationEvent(IDomainEvent domainEvent)
    {
        EventName = domainEvent.GetType().Name;
        JsonPayLoad = JsonSerializer.Serialize(domainEvent);
    }
    
    private IntegrationEvent(){}

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    
    [MaxLength(200)]
    public string EventName { get; private set; }

    public string? JsonPayLoad { get; private set; } = string.Empty;
    

    public static IntegrationEvent FromDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
        {
            throw new ArgumentNullException(nameof(domainEvent));
        }

        return new IntegrationEvent(domainEvent);
    }
}