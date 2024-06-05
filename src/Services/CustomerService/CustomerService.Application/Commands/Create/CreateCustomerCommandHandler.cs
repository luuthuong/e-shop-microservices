using Core.CQRS.Command;
using Core.EF;
using Core.Http;
using CustomerService.Infrastructure.Persistence;
using Domain;

namespace Application.Commands;

internal sealed class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork<CustomerDbContext> unitOfWork,
    IHttpRequest httpRequest
) : ICommandHandler<CreateCustomerCommand>
{
    public async Task Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            new(
                request.Name,
                request.Email,
                Address.From(request.ShippingAddress),
                CreditLimit.From(request.CreditLimit)
            )
        );

        await  customerRepository.InsertAsync(customer, cancellationToken: cancellationToken);
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }

    private Task CreateCustomerUserLogin()
    {
        return Task.CompletedTask;
    }
}
