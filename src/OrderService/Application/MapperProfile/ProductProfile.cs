using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.MapperProfile;

public  class ProductProfile: Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>().ConvertUsing((e, d) => new ProductDTO
        {
            Id = e.Id,
            CategoryId = e.CategoryId,
            Name = e.Name,
            Count = e.Count,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate,
            Price = e.Price
        });
    }
}