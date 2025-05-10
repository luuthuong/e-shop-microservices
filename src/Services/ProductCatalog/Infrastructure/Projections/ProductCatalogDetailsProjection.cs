using Core.Infrastructure.EF.Projections;
using ProductCatalog.Domain.Product.Events;
using ProductCatalog.Infrastructure.Models;

namespace ProductCatalog.Infrastructure.Projections;

public class ProductCatalogDetailsProjection : StreamProjection<ProductReadModel, ProductCatalogReadDbContext>
{
    public ProductCatalogDetailsProjection(ProductCatalogReadDbContext dbContext) : base(dbContext)
    {
        ProjectEvent<ProductCreatedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<ProductUpdatedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<ProductActivatedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<ProductDeactivatedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<StockReservedEvent>((@event, item) => item.Apply(@event));
    }
}