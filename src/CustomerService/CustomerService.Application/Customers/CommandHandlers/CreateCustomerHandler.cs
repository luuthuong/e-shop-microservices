using Application.Customers.Commands;
using Core.CQRS.Command;
using Domain.Customers;

namespace Application.Customers.CommandHandlers;

public class CreateCustomerHandler: ICommandHandler<CreateCustomer>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task Handle(CreateCustomer request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            new(
            request.Name,
            request.Email,
            request.ShippingAddress
        ));
        return _customerRepository.InsertAsync(customer, cancellationToken: cancellationToken);
    }
}