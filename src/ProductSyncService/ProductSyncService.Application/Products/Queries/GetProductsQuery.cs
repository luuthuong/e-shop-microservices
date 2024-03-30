using Core.CQRS.Query;
using Core.Models;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products.Queries;

public sealed record GetProductsQuery(
    int PageSize, 
    int PageIndex, 
    string Keyword, 
    string OrderBy = "", 
    bool Descending = false) : PageRequest(PageSize, PageIndex, OrderBy, Descending), IQuery<IEnumerable<ProductDTO>>;