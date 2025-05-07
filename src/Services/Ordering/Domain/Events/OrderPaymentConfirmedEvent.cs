using Core.Domain;

namespace Ordering.Domain.Events;

public record OrderPaymentConfirmedEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);