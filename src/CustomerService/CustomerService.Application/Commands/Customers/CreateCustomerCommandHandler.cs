using Core.CQRS.Command;
using Domain.Customers;

namespace Application.Commands.Customers;

internal sealed class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository
) : ICommandHandler<CreateCustomerCommand>
{
    public Task Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            new(
                request.Name,
                request.Email,
                request.ShippingAddress
            )
        );
        return customerRepository.InsertAsync(customer, cancellationToken: cancellationToken);
    }
}