using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderSubmittedEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);
