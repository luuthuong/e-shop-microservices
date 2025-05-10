using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Infrastructure;

namespace OrderManagement.Application.Queries.GetOrders;

public class GetOrdersQueryHandler(
    OrderReadDbContext dbContext,
    ILogger<GetOrdersQueryHandler> logger)
    : IRequestHandler<GetOrdersQuery, List<OrderSummaryDto>>
{
    public async Task<List<OrderSummaryDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting orders list with filters: StartDate={StartDate}, EndDate={EndDate}, Status={Status}, Page={Page}, PageSize={PageSize}",
            request.StartDate, request.EndDate, request.Status, request.Page, request.PageSize);

        // Build query with filters
        var query = dbContext.Orders.AsQueryable();

        // Apply filters
        if (request.StartDate.HasValue)
        {
            query = query.Where(o => o.OrderDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(o => o.OrderDate <= request.EndDate.Value);
        }

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
                CustomerEmail = o.Metadata.FirstOrDefault(m => m.Key == "CustomerEmail")!.Value,
                ItemCount = o.Items.Count
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Map to DTOs
        var result = orders.Select(o=>OrderSummaryDto.From(o.Order, o.CustomerEmail, o.ItemCount)).ToList();

        logger.LogInformation("Retrieved {OrderCount} orders", result.Count);

        return result;
    }
}