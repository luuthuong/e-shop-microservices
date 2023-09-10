using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Domain.Profiles;

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