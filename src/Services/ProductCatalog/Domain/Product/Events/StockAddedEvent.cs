using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record StockAddedEvent(int Quantity, int NewAvailableStock, Guid AggregateId, int Version)
    : DomainEvent(AggregateId, Version);