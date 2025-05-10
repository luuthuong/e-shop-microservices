using OrderManagement.Domain;

namespace OrderManagement.Infrastructure.Models;

public class OrderShippingAddressReadModel
{
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }
    public required string Country { get; init; }
    public required string ZipCode { get; init; }
    public required string RecipientName { get; init; }
    public required string PhoneNumber { get; init; }

    public static OrderShippingAddressReadModel Create(Guid orderId, Address address)
    {
        return new OrderShippingAddressReadModel
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