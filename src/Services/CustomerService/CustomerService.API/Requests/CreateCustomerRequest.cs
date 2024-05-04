namespace API.Requests;

internal sealed record CreateCustomerRequest(
    string Name,
    string Email,
    string Password,
    string PasswordConfirm,
    string ShippingAddress,
    decimal CreditLimit
);