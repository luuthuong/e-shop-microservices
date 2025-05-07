using Core.Domain;
using Core.Exception;
using MediatR;
using Ordering.Domain;

namespace Ordering.Application.Commands;

public class SubmitOrderCommandHandler(ILogger<SubmitOrderCommandHandler> logger, IEventStore<Domain.Order> orderEventStore)
    : IRequestHandler<SubmitOrderCommand>
{
    public async Task Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Submitting order {OrderId}", request.OrderId);

        var order = await orderEventStore.LoadAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException($"Ordering with ID {request.OrderId} not found");
        }

        order.SubmitOrder();
        await orderEventStore.SaveAsync(order);

        logger.LogInformation("Ordering {OrderId} submitted successfully", request.OrderId);
    }
}