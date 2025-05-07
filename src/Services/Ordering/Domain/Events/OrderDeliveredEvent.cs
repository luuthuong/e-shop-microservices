using Core.Domain;

namespace Ordering.Domain.Events;

public record OrderDeliveredEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);