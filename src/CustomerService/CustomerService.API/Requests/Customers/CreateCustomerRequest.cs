namespace API.Requests.Customers;

internal sealed record CreateCustomerRequest(
    string Name,
    string Email,
    string Password,
    string PasswordConfirm,
    string ShippingAddress,
    decimal CreditLimit
);