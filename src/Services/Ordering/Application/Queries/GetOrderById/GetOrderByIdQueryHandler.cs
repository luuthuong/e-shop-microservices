using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.DTOs;
using Ordering.Infrastructure;

namespace Ordering.Application.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(
    OrderReadDbContext dbContext,
    ILogger<GetOrderByIdQueryHandler> logger)
    : IRequestHandler<GetOrderByIdQuery, OrderDetailDto>
{
    public async Task<OrderDetailDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting order details for order {OrderId}", request.OrderId);

        var order = await dbContext.Orders
            .Include(o => o.Items)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Metadata)
            .Include(o => o.History)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            logger.LogWarning("Ordering {OrderId} not found", request.OrderId);
            return null;
        }

        var payment = await dbContext.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.OrderId == request.OrderId, cancellationToken);

        var shipment = await dbContext.Shipments
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.OrderId == request.OrderId, cancellationToken);

        var result = OrderDetailDto.FromOrder(order, payment, shipment);
        
        logger.LogInformation("Retrieved order details for order {OrderId}", request.OrderId);

        return result;
    }
}