using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.Queries.GetProduct;

public class GetProductQueryHandler(ProductCatalogReadDbContext productCatalogReadDbContext)
    : IRequestHandler<GetProductQuery, ProductDTO>
{
    public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await productCatalogReadDbContext.Products.FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            return null!;
        }

        var productDto = ProductDTO.FromProduct(product);

        return productDto;
    }
}