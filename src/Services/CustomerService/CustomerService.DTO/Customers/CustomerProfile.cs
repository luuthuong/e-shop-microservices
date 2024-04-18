using AutoMapper;
using Domain.Customers;

namespace CustomerService.DTO.Customers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDTO>().ConvertUsing(e => new(
            e.Id.Value,
            e.Name,
            e.Email,
            e.ShippingAddress.StreetAddress
        ));
    }
}