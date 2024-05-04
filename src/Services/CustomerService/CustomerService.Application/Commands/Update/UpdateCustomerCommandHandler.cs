using Core.CQRS.Command;

namespace Application.Customers;

public class UpdateCustomerCommandHandler: ICommandHandler<UpdateCustomerCommand>
{
    public Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        return default;
    }
}