using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record StockReservedEvent(
    Guid OrderId,
    int Quantity,
    int RemainingStock,
    Guid AggregateId,
    int Version) : DomainEvent(AggregateId, Version);