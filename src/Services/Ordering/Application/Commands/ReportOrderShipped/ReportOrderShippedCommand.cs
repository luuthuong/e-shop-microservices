using MediatR;

namespace Ordering.Application.Commands;

public class ReportOrderShippedCommand : IRequest
{
    public Guid OrderId { get; set; }
    public Guid ShipmentId { get; set; }
    public string TrackingNumber { get; set; }
    public DateTime ShippedDate { get; set; }
}