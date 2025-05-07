using Core.Domain;

namespace Ordering.Domain.Events;

public record OrderShippedEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);