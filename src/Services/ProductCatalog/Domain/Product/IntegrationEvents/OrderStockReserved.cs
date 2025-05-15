using Core.Domain;

namespace ProductCatalog.Domain.Product.IntegrationEvents;

public record OrderStockReserved(Guid OrderId);