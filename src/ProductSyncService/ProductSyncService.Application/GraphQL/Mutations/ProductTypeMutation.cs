using AutoMapper;
using EntityGraphQL.Schema;
using ProductSyncService.Domain.Entities;
using ProductSyncService.DTO;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.GraphQL.Mutations;

public sealed class ProductTypeMutation
{
    private readonly IProductTypeRepository _productTypeRepository;
    private readonly IMapper _mapper;

    public ProductTypeMutation(IProductTypeRepository productTypeRepository, IMapper mapper)
    {
        _productTypeRepository = productTypeRepository;
        _mapper = mapper;
    }

    [GraphQLMutation("update product type")]
    public async Task<ProductTypeDTO> CreateType( [GraphQLArguments] CreateProductTypeRequest request)
    {
        var product = ProductType.Create(
            request.Name,
            request.ParentId,
            request.Description
        );
        var result = await _productTypeRepository.InsertAsync(product);

        return _mapper.Map<ProductType, ProductTypeDTO>(result);
    } 
}