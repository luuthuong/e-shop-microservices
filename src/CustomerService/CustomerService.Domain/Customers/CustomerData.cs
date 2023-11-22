namespace Domain.Customers;

public sealed record class CustomerData(
    string Email,
    string Name,
    string ShippingAddress
    );