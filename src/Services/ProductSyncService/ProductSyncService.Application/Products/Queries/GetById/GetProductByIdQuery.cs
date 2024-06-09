using Core.CQRS.Query;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products;

public sealed record GetProductByIdQuery(ProductId ProductId) : IQueryCache<ProductDTO>
{
    public bool BypassCache { get; }
    public string CacheKey => $"Product:{ProductId.Value}";
}