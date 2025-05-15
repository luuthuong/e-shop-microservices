using Core.Domain;

namespace ProductCatalog.Domain.Product.IntegrationEvents;

public record OrderOutOfStock(
    Guid OrderId,
    IEnumerable<Guid> Items
);