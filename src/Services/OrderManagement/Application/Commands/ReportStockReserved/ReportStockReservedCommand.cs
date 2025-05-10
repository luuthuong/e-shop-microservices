using MediatR;

namespace OrderManagement.Application.Commands;

public class ReportStockReservedCommand : IRequest
{
    public Guid OrderId { get; set; }
}