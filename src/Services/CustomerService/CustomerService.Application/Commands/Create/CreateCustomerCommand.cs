using Core.CQRS.Command;

namespace Application.Commands;

public sealed record CreateCustomerCommand(
    string Email,
    string Password,
    string PasswordConfirm,
    string Name,
    string ShippingAddress,
    decimal CreditLimit
) : ICommand;