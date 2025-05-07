using MediatR;

namespace Ordering.Application.Commands;

public record ReportOrderDeliveredCommand : IRequest
{
    public Guid OrderId { get; set; }
    public DateTime DeliveredDate { get; set; }
}