using MediatR;
using Ordering.Application.DTOs;

namespace Ordering.Application.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDetailDto>
{
    public Guid OrderId { get; set; }
}