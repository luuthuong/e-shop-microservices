using Core.CQRS.Query;
using Core.Models;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products;

public sealed record GetProductsQuery(
    int PageSize, 
    int PageIndex, 
    string Keyword, 
    string OrderBy = "", 
    bool Descending = false) : PageRequest(PageSize, PageIndex, OrderBy, Descending), IQueryCache<IEnumerable<ProductDTO>>
{
    public bool BypassCache { get; }
    public string CacheKey => $"PageIndex.{PageIndex}_PageSize.{PageSize}";
}