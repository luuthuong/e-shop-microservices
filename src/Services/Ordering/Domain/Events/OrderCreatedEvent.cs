using Core.Domain;

namespace Ordering.Domain.Events;

public record OrderCreatedEvent(
    Guid AggregateId,
    int Version,
    Guid CustomerId,
    List<OrderItem> Items,
    Money TotalAmount,
    DateTime OrderDate,
    Address ShippingAddress
) : DomainEvent(AggregateId, Version);