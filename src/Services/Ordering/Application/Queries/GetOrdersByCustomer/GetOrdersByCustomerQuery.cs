using MediatR;
using Ordering.Application.DTOs;

namespace Ordering.Application.Queries.GetOrdersByCustomer;

public sealed class GetOrdersByCustomerQuery : IRequest<List<OrderSummaryDto>>
{
    public Guid CustomerId { get; set; }
    public string Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
