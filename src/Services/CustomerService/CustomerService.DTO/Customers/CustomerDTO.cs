namespace CustomerService.DTO.Customers;

public sealed record CustomerDTO(
    Guid Id,
    string Name,
    string Email,
    string ShippingAddress
);