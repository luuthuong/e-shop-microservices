using AutoMapper;
using Core.CQRS.Query;
using Core.Exception;
using ProductSyncService.Application.Products.Queries;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products.QueryHandlers;

internal sealed class GetProductByIdHandler: IQueryHandler<GetProductById, ProductDTO?>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<ProductDTO?> Handle(GetProductById request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new DomainLogicException(ProductError.NotFound.Description);
        return _mapper.Map<Product, ProductDTO>(product);
    }
}