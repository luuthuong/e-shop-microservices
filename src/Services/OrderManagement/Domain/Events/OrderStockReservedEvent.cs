using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderStockReservedEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);