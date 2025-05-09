using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.Queries.GetProducts;

public class GetProductsQueryHandler(ProductCatalogReadDbContext productCatalogReadDbContext)
    : IRequestHandler<GetProductsQuery, IEnumerable<ProductDTO>>
{
    public async Task<IEnumerable<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productCatalogReadDbContext.Products.ToListAsync(cancellationToken);
        return products.Select(ProductDTO.FromProduct);
    }
}