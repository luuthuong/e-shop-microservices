using Core.BaseDomain;

namespace Domain.DomainEvents.Products;

public record UnPublishProductDomainEvent(
    Guid ProductId,
    Guid CategoryId
    ): IDomainEvent;