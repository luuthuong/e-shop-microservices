using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Infrastructure;

namespace OrderManagement.Application.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerQueryHandler(
    OrderReadDbContext dbContext,
    ILogger<GetOrdersByCustomerQueryHandler> logger)
    : IRequestHandler<GetOrdersByCustomerQuery, List<OrderSummaryDto>>
{
    public async Task<List<OrderSummaryDto>> Handle(GetOrdersByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting orders for customer {CustomerId}, Status={Status}, Page={Page}, PageSize={PageSize}",
            request.CustomerId, request.Status, request.Page, request.PageSize);

        // Build query with customer filter
        var query = dbContext.Orders
            .Where(o => o.CustomerId == request.CustomerId);

        // Apply status filter if provided
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(o => o.Status == request.Status);
        }

        // Calculate pagination
        int skip = (request.Page - 1) * request.PageSize;

        // Execute query with pagination and ordering
        var orders = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip(skip)
            .Take(request.PageSize)
            .Select(o => new
            {
                Order = o,
                CustomerEmail = o.Metadata.FirstOrDefault(m => m.Key == "CustomerEmail").Value,
                ItemCount = o.Items.Count
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Map to DTOs
        var result = orders.Select(o => new OrderSummaryDto
        {
            Id = o.Order.Id,
            CustomerId = o.Order.CustomerId,
            CustomerEmail = o.CustomerEmail,
            Status = o.Order.Status,
            TotalAmount = o.Order.TotalAmount,
            Currency = o.Order.Currency,
            OrderDate = o.Order.OrderDate,
            ItemCount = o.ItemCount
        }).ToList();

        logger.LogInformation("Retrieved {OrderCount} orders for customer {CustomerId}",
            result.Count, request.CustomerId);

        return result;
    }
}