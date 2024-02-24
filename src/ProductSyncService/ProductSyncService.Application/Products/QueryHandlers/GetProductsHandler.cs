using AutoMapper;
using Core.CQRS.Query;
using ProductSyncService.Application.Products.Queries;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products.QueryHandlers;

public class GetProductsHandler(
    IProductRepository productRepository, 
    IMapper mapper
)
    : IQueryHandler<GetProducts, IEnumerable<ProductDTO>>
{
    public async Task<IEnumerable<ProductDTO>> Handle(GetProducts request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetListAsync(cancellationToken);
        return  mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
    }
}