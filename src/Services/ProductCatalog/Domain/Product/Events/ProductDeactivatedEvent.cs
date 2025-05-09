using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record ProductDeactivatedEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);