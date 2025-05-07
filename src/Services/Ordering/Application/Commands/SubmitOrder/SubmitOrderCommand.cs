using MediatR;

namespace Ordering.Application.Commands;

public class SubmitOrderCommand : IRequest
{
    public Guid OrderId { get; set; }
}
