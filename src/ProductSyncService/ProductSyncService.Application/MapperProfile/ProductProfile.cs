using Application.DTO;
using AutoMapper;
using ProductSyncService.Application.DTO;
using ProductSyncService.Domain.Entities;

namespace ProductSyncService.Application.MapperProfile;

public  class ProductProfile: Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>().ConvertUsing((e, d) => new ProductDTO
        {
            Id = e.Id,
            Name = e.Name,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate,
        });
    }
}