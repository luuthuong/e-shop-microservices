using Core.CQRS.Query;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products;

public sealed record GetProductByIdQuery(ProductId ProductId) : IQuery<ProductDTO>;