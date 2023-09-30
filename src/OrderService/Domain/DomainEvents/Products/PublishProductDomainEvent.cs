using Core.BaseDomain;

namespace Domain.DomainEvents.Products;

public record PublishProductDomainEvent(
    Guid ProductId,
    Guid CategoryId
    ): IDomainEvent;