namespace Core.Domain;

public abstract record DomainEvent(Guid AggregateId, int Version)
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}