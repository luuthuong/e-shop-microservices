using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Domain.Profiles;

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