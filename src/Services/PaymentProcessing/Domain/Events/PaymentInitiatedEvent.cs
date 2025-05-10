using Core.Domain;

namespace PaymentProcessing.Domain.Events;

public record PaymentInitiatedEvent(
    Guid AggregateId,
    int Version,
    Guid OrderId,
    string CustomerId,
    decimal Amount,
    string Currency,
    string PaymentMethod) : DomainEvent(AggregateId, Version);