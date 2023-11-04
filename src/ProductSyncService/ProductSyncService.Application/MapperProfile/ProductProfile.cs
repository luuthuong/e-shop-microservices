using AutoMapper;
using ProductSyncService.Domain.Entities;
using ProductSyncService.DTO;

namespace ProductSyncService.Application.MapperProfile;

public sealed class ProductProfile: Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>().ConvertUsing((e, d) => new ProductDTO
        {
            Id = e.Id,
            CategoryId = e.ProductTypeId,
            CategoryName = e.ProductType?.Name ?? string.Empty,
            Name = e.Name,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate,
        });
    }
}