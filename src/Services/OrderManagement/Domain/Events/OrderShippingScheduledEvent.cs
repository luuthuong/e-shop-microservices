using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderShippingScheduledEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);