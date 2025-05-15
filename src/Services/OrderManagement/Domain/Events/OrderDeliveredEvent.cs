using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderDeliveredEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);