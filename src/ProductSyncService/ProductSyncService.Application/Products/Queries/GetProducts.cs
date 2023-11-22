using Core.CQRS.Query;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products.Queries;

public sealed record GetProducts: IQuery<IEnumerable<ProductDTO>>
{
    private GetProducts()
    {
        
    }

    public static GetProducts Create() => new();
}