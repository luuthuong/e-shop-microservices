using Core.Domain;

namespace PaymentProcessing.Domain.Events;

public record PaymentFailedEvent(Guid AggregateId, int Version, string FailureReason, DateTime ProcessedDate)
    : DomainEvent(AggregateId, Version);