using AutoMapper;
using ProductSyncService.Application.DTO;
using ProductSyncService.Application.Helpers;
using ProductSyncService.Domain.Entities;
using ProductSyncService.DTO;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.CQRS.Products.Queries;

public record GetProductsQuery() : BaseRequest<GetPagingProductResponse>;

internal sealed class GetProductsQueryHandler: BaseRequestHandler<GetProductsQuery, GetPagingProductResponse>
{
    private readonly IProductRepository _productRepository;
    public GetProductsQueryHandler(IMapper mapper, IAppDbContext dbContext, IProductRepository productRepository) : base(mapper, dbContext)
    {
        _productRepository = productRepository;
    }


    public override async Task<GetPagingProductResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetListAsync(cancellationToken);
        var data = Mapper.Map<IEnumerable<Product>, IList<ProductDTO>>(products.ToList());

        return new()
        {
            Success = true,
            Data = new ()
            {
                Data = data,
                Total = data.Count
            }
        };
    }
}