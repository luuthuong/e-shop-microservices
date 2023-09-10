using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Domain.Profiles;

public  class ProductProfile: Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>().ConvertUsing((e, d) => new ProductDTO
        {
            Id = e.Id,
            Name = e.Name,
            Count = e.Count,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate,
            Price = e.Price
        });
    }
}