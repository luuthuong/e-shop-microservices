using Core.CQRS.Command;
using Domain;

namespace Application.Commands;

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
                Address.From(request.ShippingAddress),
                CreditLimit.From(request.CreditLimit)
            )
        );
        return customerRepository.InsertAsync(customer, cancellationToken: cancellationToken);
    }

    private Task CreateCustomerUserLogin()
    {
        return Task.CompletedTask;
    }
}