using Core.Domain;

namespace Ordering.Domain.Events;

public record ShippingAddressSetEvent(Guid AggregateId, int Version, Address Address)
    : DomainEvent(AggregateId, Version);