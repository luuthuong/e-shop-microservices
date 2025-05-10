using Core.Domain;

namespace OrderManagement.Domain.Events;

public record OrderMetadataAddedEvent(Guid AggregateId, int Version, string Key, string Value)
    : DomainEvent(AggregateId, Version);