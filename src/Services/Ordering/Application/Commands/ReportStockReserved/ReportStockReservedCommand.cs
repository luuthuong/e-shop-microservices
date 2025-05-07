using MediatR;

namespace Ordering.Application.Commands;

public class ReportStockReservedCommand : IRequest
{
    public Guid OrderId { get; set; }
}