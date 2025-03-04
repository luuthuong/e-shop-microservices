using Core.CQRS.Command;
using Domain.Commands;

namespace Application.Orders.ProcessingOrder;

public class ProcessOrderHandler : ICommandHandler<ProcessOrder>
{
    public async Task Handle(ProcessOrder command, CancellationToken cancellationToken)
    {
    }
}