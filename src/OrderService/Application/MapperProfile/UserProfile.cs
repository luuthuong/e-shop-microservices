using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.MapperProfile;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>().ConvertUsing(e => new UserDTO
        {
            Id = e.Id,
            UserName = e.UserName,
            Email = e.Email,
            DisplayName = e.DisplayName,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate
        });
    }
}