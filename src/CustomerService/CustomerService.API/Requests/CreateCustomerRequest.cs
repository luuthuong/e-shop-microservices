namespace API.Requests;

public record CreateCustomerRequest(
    string Name,
    string Email,
    string Password,
    string PasswordConfirm,
    string ShippingAddress
);