using Application.Customers.Commands;
using Core.CQRS.Command;
using Domain.Customers;

namespace Application.Customers.CommandHandlers;

public class CreateCustomerHandler(ICustomerRepository customerRepository) : ICommandHandler<CreateCustomer>
{
    public Task Handle(CreateCustomer request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            new(
            request.Name,
            request.Email,
            request.ShippingAddress
        ));
        return customerRepository.InsertAsync(customer, cancellationToken: cancellationToken);
    }
}