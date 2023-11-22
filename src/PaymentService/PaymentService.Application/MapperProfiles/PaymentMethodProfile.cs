using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.MapperProfiles;

public class PaymentMethodProfile: Profile
{
    public PaymentMethodProfile()
    {
        CreateMap<PaymentMethod, PaymentMethodDTO>().ConstructUsing(e => new(
            e.Id.Value,
            e.Name,
            e.Description,
            e.CreatedDate,
            e.UpdatedDate
            )
        );
    }
}