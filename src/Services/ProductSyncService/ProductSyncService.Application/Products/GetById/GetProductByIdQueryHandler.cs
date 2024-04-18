using AutoMapper;
using Core.CQRS.Query;
using Core.Exception;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products;
internal sealed class GetProductByIdQueryHandler(IMapper mapper, IProductRepository productRepository)
    : IQueryHandler<GetProductByIdQuery, ProductDTO?>
{
    public async Task<ProductDTO?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new DomainLogicException(ProductError.NotFound.Description);
        return mapper.Map<Product, ProductDTO>(product);
    }
}