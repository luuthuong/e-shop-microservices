using Core.Domain;

namespace Ordering.Domain.Events;

public record OrderCancelledEvent(Guid AggregateId, int Version, string Reason) : DomainEvent(AggregateId, Version);