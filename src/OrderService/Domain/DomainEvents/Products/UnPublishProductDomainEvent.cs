using Domain.Base;

namespace Domain.DomainEvents.Products;

public record UnPublishProductDomainEvent(
    Guid ProductId,
    Guid CategoryId
    ): IDomainEvent
{
    
}