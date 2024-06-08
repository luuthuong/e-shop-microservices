using AutoMapper;
using Core.CQRS.Query;
using Core.Infrastructure.AutoMapper;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products;
internal sealed class GetProductsQueryHandler(
    IProductRepository productRepository,
    IMapper mapper
) : IQueryHandler<GetProductsQuery, IEnumerable<ProductDTO>>
{
    public async Task<IEnumerable<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetListAsync(cancellationToken);
        return mapper.MapEnumerable<Product, ProductDTO>(products);
    }
}