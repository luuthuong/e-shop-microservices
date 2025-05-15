using System.Text.Json;

namespace Core.Domain;

public class EventOutbox
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid AggregateId { get; private set; }
    public string EventType { get; private set; } = string.Empty;
    public string EventData { get; private set; } = string.Empty;
    public DateTime Created { get; private set; } = DateTime.UtcNow;
    public DateTime? Processed { get; private set; } = null;
    public int RetryCount { get; private set; } = 0;

    public static EventOutbox FromDomainEvent(DomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event, nameof(@event));
       
        if (@event.AggregateId == Guid.Empty)
        {
            throw new ArgumentException("Aggregate ID cannot be empty.", nameof(@event));
        }

        return new()
        {
            AggregateId = @event.AggregateId,
            EventType = @event.GetType().AssemblyQualifiedName!,
            EventData = JsonSerializer.Serialize(@event, @event.GetType())
        };
    }
}