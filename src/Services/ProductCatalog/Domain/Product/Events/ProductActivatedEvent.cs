using Core.Domain;

namespace ProductCatalog.Domain.Product.Events;

public record ProductActivatedEvent(Guid AggregateId, int Version) : DomainEvent(AggregateId, Version);