using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderOutOfStockEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);