using AutoMapper;
using Core.CQRS.Query;
using Core.Exception;
using ProductSyncService.Application.Products.Queries;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products.QueryHandlers;
internal sealed class GetProductByIdHandler(IMapper mapper, IProductRepository productRepository)
    : IQueryHandler<GetProductById, ProductDTO?>
{
    public async Task<ProductDTO?> Handle(GetProductById request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new DomainLogicException(ProductError.NotFound.Description);
        return mapper.Map<Product, ProductDTO>(product);
    }
}