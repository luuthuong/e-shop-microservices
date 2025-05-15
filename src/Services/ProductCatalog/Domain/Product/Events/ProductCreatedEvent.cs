using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record ProductCreatedEvent(
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    int AvailableStock,
    string Category,
    Guid AggregateId,
    int Version) : DomainEvent(AggregateId, Version);