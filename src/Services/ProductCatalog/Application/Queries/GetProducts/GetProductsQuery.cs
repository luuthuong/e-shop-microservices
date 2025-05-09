using MediatR;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Queries.GetProducts;

public record GetProductsQuery: IRequest<IEnumerable<ProductDTO>>
{
    
}