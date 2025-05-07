using Core.Infrastructure.EF.Projections;
using Ordering.Domain.Events;
using Ordering.Infrastructure.Models;

namespace Ordering.Infrastructure.Projections;

sealed class OrderDetailsProjection : StreamProjection<OrderReadModel, OrderReadDbContext>
{
    public OrderDetailsProjection(OrderReadDbContext dbContext) : base(dbContext)
    {
        ProjectEvent<OrderCreatedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<OrderSubmittedEvent>((@event, item) => item.Apply(@event));
    }
}