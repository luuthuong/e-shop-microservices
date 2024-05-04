using AutoMapper;
using Domain;

namespace CustomerService.DTO.Customers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDTO>().ConvertUsing(e => new(
            e.Id.Value,
            e.Name,
            e.Email,
            e.Address.StreetAddress
        ));
    }
}