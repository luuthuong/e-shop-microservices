using Core.Infrastructure.EF.Projections;
using OrderManagement.Domain.Events;
using OrderManagement.Infrastructure.Models;

namespace OrderManagement.Infrastructure.Projections;

sealed class OrderDetailsProjection : StreamProjection<OrderReadModel, OrderReadDbContext>
{
    public OrderDetailsProjection(OrderReadDbContext dbContext) : base(dbContext)
    {
        ProjectEvent<OrderCreatedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<OrderSubmittedEvent>((@event, item) => item.Apply(@event));
    }
}