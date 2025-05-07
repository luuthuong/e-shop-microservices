using Ordering.Infrastructure.Models;

namespace Ordering.Application.DTOs;

public class ShippingAddressDto
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }
    public string RecipientName { get; private set; }
    public string PhoneNumber { get; private set; }

    public static ShippingAddressDto? FromShippingAddress(OrderShippingAddressReadModel? address)
    {
        if (address is null)
        {
            return null;
        }
        
        return new ShippingAddressDto
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country,
            ZipCode = address.ZipCode,
            RecipientName = address.RecipientName,
            PhoneNumber = address.PhoneNumber
        };
    }
}