using Core.CQRS.Command;
using Domain.Commands;

namespace Application.Orders.CompletingOrder;

public class CompleteOrderHandler : ICommandHandler<CompleteOrder>
{

    public async Task Handle(CompleteOrder command, CancellationToken cancellationToken)
    {
    }
}