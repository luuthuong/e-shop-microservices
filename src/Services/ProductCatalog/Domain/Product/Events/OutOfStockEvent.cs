using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record OutOfStockEvent(
    Guid OrderId,
    int RequestedQuantity,
    int AvailableStock,
    Guid AggregateId,
    int Version) : DomainEvent(AggregateId, Version);