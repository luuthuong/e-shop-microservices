using Core.Domain;
using Core.Exception;
using MediatR;
using Ordering.Domain;

namespace Ordering.Application.Commands;

public class ReportOrderDeliveredCommandHandler(
    IEventStore<Domain.Order> orderEventStore,
    ILogger<ReportOrderDeliveredCommandHandler> logger)
    : IRequestHandler<ReportOrderDeliveredCommand>
{
    public async Task Handle(ReportOrderDeliveredCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Reporting order {OrderId} delivered", request.OrderId);

        var order = await orderEventStore.LoadAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException($"Ordering with ID {request.OrderId} not found");
        }

        order.AddMetadata("DeliveredDate", request.DeliveredDate.ToString("o"));

        order.ReportDelivered();

        await orderEventStore.SaveAsync(order);

        logger.LogInformation("Ordering {OrderId} marked as delivered", request.OrderId);
    }
}