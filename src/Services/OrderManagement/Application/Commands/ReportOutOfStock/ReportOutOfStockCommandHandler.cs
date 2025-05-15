using Core.Domain;
using Core.Exception;
using MediatR;
using OrderManagement.Domain;

namespace OrderManagement.Application.Commands;

public class ReportOutOfStockCommandHandler(
    IEventStore<Domain.Order> orderEventStore,
    ILogger<ReportOutOfStockCommandHandler> logger)
    : IRequestHandler<ReportOutOfStockCommand>
{
    public async Task Handle(ReportOutOfStockCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Reporting out of stock for order {OrderId}", request.OrderId);

        var order = await orderEventStore.LoadAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException($"OrderManagement with ID {request.OrderId} not found");
        }

        order.AddMetadata("OutOfStockProductIds", string.Join(",", request.OutOfStockProductIds));

        order.ReportOutOfStock();

        await orderEventStore.SaveAsync(order);

        logger.LogInformation("Out of stock reported for order {OrderId}", request.OrderId);
    }
}