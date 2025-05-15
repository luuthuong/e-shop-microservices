using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderCancelledEvent(Guid AggregateId, int Version, string Reason) : DomainEvent(AggregateId, Version);