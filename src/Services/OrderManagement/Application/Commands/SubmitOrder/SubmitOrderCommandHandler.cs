using Core.Domain;
using Core.Exception;
using MediatR;
using OrderManagement.Domain;

namespace OrderManagement.Application.Commands;

public class SubmitOrderCommandHandler(ILogger<SubmitOrderCommandHandler> logger, IEventStore<Domain.Order> orderEventStore)
    : IRequestHandler<SubmitOrderCommand>
{
    public async Task Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Submitting order {OrderId}", request.OrderId);

        var order = await orderEventStore.LoadAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException($"OrderManagement with ID {request.OrderId} not found");
        }

        order.SubmitOrder();
        await orderEventStore.SaveAsync(order);

        logger.LogInformation("OrderManagement {OrderId} submitted successfully", request.OrderId);
    }
}