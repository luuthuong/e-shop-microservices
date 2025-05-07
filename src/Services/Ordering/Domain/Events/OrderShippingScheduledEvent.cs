using Core.Domain;

namespace Ordering.Domain.Events;

public record OrderShippingScheduledEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);