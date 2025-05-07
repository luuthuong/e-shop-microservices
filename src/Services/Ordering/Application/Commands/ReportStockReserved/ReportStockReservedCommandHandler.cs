using Core.Domain;
using Core.Exception;
using MediatR;
using Ordering.Domain;

namespace Ordering.Application.Commands;

public class ReportStockReservedCommandHandler(
    IEventStore<Domain.Order> orderEventStore,
    ILogger<ReportStockReservedCommandHandler> logger
) : IRequestHandler<ReportStockReservedCommand>
{
    public async Task Handle(ReportStockReservedCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Reporting stock reserved for order {OrderId}", request.OrderId);

        var order = await orderEventStore.LoadAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException($"Order with ID {request.OrderId} not found");
        }

        order.ReportStockReserved();

        await orderEventStore.SaveAsync(order);
        logger.LogInformation("Stock reservation reported for order {OrderId}", request.OrderId);
    }
}