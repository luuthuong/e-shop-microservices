using Core.Domain;
using ProductSyncService.Domain.Categories;

namespace ProductSyncService.Domain.Products.Events;

public sealed record ProductCategoryUpdated(
    ProductId ProductId,
    CategoryId CategoryId,
    CategoryId NewCategoryId
) : IDomainEvent;
