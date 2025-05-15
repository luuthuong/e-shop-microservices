using Core.Domain;

namespace OrderManagement.Domain.Events;

public record ShippingAddressSetEvent(Guid AggregateId, int Version, Address Address)
    : DomainEvent(AggregateId, Version);