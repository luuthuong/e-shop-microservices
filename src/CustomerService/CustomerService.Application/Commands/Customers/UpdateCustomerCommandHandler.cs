using Core.CQRS.Command;

namespace Application.Commands.Customers;

public class UpdateCustomerCommandHandler: ICommandHandler<UpdateCustomerCommand>
{
    public Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        return default;
    }
}