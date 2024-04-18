using Core.CQRS.Command;

namespace Application.Customers;

public sealed record CreateCustomerCommand(
    string Email,
    string Password,
    string PasswordConfirm,
    string Name,
    string ShippingAddress,
    decimal CreditLimit
) : ICommand;