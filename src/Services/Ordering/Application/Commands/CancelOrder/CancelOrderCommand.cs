using MediatR;

namespace Ordering.Application.Commands;

public class CancelOrderCommand : IRequest
{
    public Guid OrderId { get; set; }
    public string Reason { get; set; }
}