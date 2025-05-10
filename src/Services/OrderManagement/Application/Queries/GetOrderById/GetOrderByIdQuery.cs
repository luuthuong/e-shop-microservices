using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDetailDto>
{
    public Guid OrderId { get; set; }
}