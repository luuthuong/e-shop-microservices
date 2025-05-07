using Core.Domain;

namespace PaymentProcessing.Domain.Events;

public record PaymentRefundedEvent(Guid AggregateId, int Version, string RefundReason, DateTime RefundDate)
    : DomainEvent(AggregateId, Version);