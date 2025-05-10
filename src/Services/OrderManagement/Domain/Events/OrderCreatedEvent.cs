using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderCreatedEvent(
    Guid AggregateId,
    int Version,
    Guid CustomerId,
    List<OrderItem> Items,
    Money TotalAmount,
    DateTime OrderDate,
    Address ShippingAddress
) : DomainEvent(AggregateId, Version);