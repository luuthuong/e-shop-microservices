using Core.CQRS.Command;
using Domain.Commands;

namespace Application.Orders.CancelingOrder;

public class CancelOrderHandler: ICommandHandler<CancelOrder>
{
    public async Task Handle(CancelOrder command, CancellationToken cancellationToken)
    { }
}