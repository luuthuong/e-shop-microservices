using MediatR;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Queries.GetProduct;

public record GetProductQuery(Guid ProductId ): IRequest<ProductDTO>;