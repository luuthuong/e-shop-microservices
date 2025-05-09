using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record StockReleasedEvent(
    Guid OrderId,
    int Quantity,
    int NewAvailableStock,
    Guid AggregateId,
    int Version) : DomainEvent(AggregateId, Version);