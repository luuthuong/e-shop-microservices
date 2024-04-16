
using Core.CQRS.Command;
using Domain.Customers;

namespace Application.Commands.Customers;

public sealed record UpdateCustomerCommand(
    CustomerId CustomerId,
    string Name,
    string ShippingAddress,
    decimal CreditLimit
) : ICommand;
