using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Queries.GetOrders;

public class GetOrdersQuery : IRequest<List<OrderSummaryDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}