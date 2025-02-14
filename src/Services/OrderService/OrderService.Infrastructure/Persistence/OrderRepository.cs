using Core.Infrastructure.EF.Repository;
using Domain;
using JetBrains.Annotations;

namespace Infrastructure.Persistence;

public class OrderRepository([NotNull] OrderDbContext dbContext) : Repository<OrderDbContext, Order, OrderId>(dbContext), IOrderRepository
{
    
}