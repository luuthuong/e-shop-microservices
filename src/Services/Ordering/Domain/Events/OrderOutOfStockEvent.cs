using Core.Domain;

namespace Ordering.Domain.Events;

public record OrderOutOfStockEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);