
using Core.CQRS.Command;
using Domain;

namespace Application.Customers;

public sealed record UpdateCustomerCommand(
    CustomerId CustomerId,
    string Name,
    string ShippingAddress,
    decimal CreditLimit
) : ICommand;
