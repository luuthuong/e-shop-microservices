using AutoMapper;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.AutoMapperProfiles.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>().ConvertUsing((e) => new()
        {
            Id = e.Id.Value,
            Name = e.Name,
            Description = e.Description,
            ShortDescription = e.ShortDescription,
            CategoryId = e.CategoryId != null ? e.CategoryId.Value : default,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate
        });
    }
}