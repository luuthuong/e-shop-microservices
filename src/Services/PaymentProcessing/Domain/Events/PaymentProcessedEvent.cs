using Core.Domain;

namespace PaymentProcessing.Domain.Events;

public record PaymentProcessedEvent(Guid AggregateId, int Version, string TransactionId, DateTime ProcessedDate)
    : DomainEvent(AggregateId, Version);