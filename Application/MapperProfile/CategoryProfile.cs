using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.MapperProfile;
public class CategoryProfile: Profile
{
    public CategoryProfile()
    {
        CreateMap<Category,CategoryDTO>().ConvertUsing(e => new CategoryDTO
        {
            Id = e.Id,
            Name = e.Name,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate
        });
    }
}