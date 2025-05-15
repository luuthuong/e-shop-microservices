using MediatR;

namespace OrderManagement.Application.Commands;

public class SubmitOrderCommand : IRequest
{
    public Guid OrderId { get; set; }
}
