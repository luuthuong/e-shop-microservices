using Core.BaseDomain;

namespace Domain.DomainEvents.Products;

public sealed record ProductCreatedDomainEvent(
    Guid ProductId,
    Guid CategoryId
): IDomainEvent;