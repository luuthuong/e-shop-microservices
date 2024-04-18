using AutoMapper;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.DTO.Products;

public sealed class ProductProfile: Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>().ConvertUsing((e, d) => new ProductDTO
        {
            Id = e.Id.Value,
            CategoryId = e?.CategoryId?.Value,
            Name = e.Name,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate,
        });
    }
}