using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record ProductCreatedEvent(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int AvailableStock,
    string PictureFileName,
    Guid CategoryId,
    string Brand,
    bool IsActive,
    Guid AggregateId,
    int Version
) : DomainEvent(AggregateId, Version);

public record ProductUpdatedEvent(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string PictureFileName,
    Guid CategoryId,
    string Brand,
    Guid AggregateId,
    int Version
) : DomainEvent(AggregateId, Version);