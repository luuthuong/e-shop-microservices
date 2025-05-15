using Core.Domain;

namespace PaymentProcessing.Domain.Events;

public record PaymentFailedEvent(
    Guid AggregateId,
    int Version,
    string FailureReason,
    string ErrorCode,
    DateTime FailedDate) : DomainEvent(AggregateId, Version);