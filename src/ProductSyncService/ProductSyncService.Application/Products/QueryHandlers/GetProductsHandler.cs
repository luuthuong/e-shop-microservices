using AutoMapper;
using Core.CQRS.Query;
using ProductSyncService.Application.Products.Queries;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products.QueryHandlers;

public class GetProductsHandler: IQueryHandler<GetProducts, IEnumerable<ProductDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    
    public GetProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> Handle(GetProducts request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetListAsync(cancellationToken);
        return  _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
    }
}