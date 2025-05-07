
using Core.Domain;
using Core.Exception;
using MediatR;
using Ordering.Domain;

namespace Ordering.Application.Commands;

public class ReportOrderShippedCommandHandler(
    IEventStore<Domain.Order> orderEventStore,
    ILogger<ReportOrderShippedCommandHandler> logger)
    : IRequestHandler<ReportOrderShippedCommand>
{
    public async Task Handle(ReportOrderShippedCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Reporting order {OrderId} shipped with shipment {ShipmentId}", 
            request.OrderId, request.ShipmentId);
            
        var order = await orderEventStore.LoadAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException($"Ordering with ID {request.OrderId} not found");
        }

        // Add shipment information to order metadata
        order.AddMetadata("ShipmentId", request.ShipmentId.ToString());
        order.AddMetadata("TrackingNumber", request.TrackingNumber);
        order.AddMetadata("ShippedDate", request.ShippedDate.ToString("o"));

        // Report order shipped
        order.ReportShipped();

        // Save the changes
        await orderEventStore.SaveAsync(order);

        logger.LogInformation("Ordering {OrderId} marked as shipped", request.OrderId);
    }
}