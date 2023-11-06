using Grpc.Core;
using Grpc.ProductSyncService;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace API.Services;

public class GrpcProductService: GrpcProduct.GrpcProductBase
{
    private readonly IProductRepository _productRepository;

    public GrpcProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
    {
        var parseSuccess = Guid.TryParse(request.Id, out Guid id);
        if (!parseSuccess)
            return default;
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return default;

        return new GetByIdResponse()
        {
            Id = product?.Id.ToString(),
            Name = product?.Name,
            TypeId = product?.ProductTypeId.ToString()
        };
    }
}