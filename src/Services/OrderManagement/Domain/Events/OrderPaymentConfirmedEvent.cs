using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderPaymentConfirmedEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);