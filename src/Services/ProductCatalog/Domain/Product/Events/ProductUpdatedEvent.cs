using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record ProductUpdatedEvent(
    string Name,
    string Description,
    decimal Price,
    int AvailableStock,
    string PictureUrl,
    string Category,
    Guid AggregateId,
    int Version) : DomainEvent(AggregateId, Version);