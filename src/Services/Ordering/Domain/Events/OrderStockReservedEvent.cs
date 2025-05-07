using Core.Domain;

namespace Ordering.Domain.Events;

public record OrderStockReservedEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);