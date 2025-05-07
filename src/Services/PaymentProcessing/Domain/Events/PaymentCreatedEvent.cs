using Core.Domain;

namespace PaymentProcessing.Domain.Events;

public record PaymentCreatedEvent(
    Guid AggregateId,
    int Version,
    Guid OrderId,
    Money Amount,
    PaymentMethod Method,
    DateTime RequestDate)
    : DomainEvent(AggregateId, Version);