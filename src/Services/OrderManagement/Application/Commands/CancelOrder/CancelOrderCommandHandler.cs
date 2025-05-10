using Core.Domain;
using Core.Exception;
using MediatR;
using OrderManagement.Domain;

namespace OrderManagement.Application.Commands;

public class CancelOrderCommandHandler(
    IEventStore<Domain.Order> orderEventStore,
    ILogger<CancelOrderCommandHandler> logger)
    : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Cancelling order {OrderId} with reason: {Reason}", request.OrderId, request.Reason);

        // Get the order
        var order = await orderEventStore.LoadAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException($"OrderManagement with ID {request.OrderId} not found");
        }

        // Cancel the order
        order.CancelOrder(request.Reason);

        await orderEventStore.SaveAsync(order);

        logger.LogInformation("OrderManagement {OrderId} cancelled successfully", request.OrderId);
    }
}