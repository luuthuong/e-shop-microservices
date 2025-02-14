using Core.EF;

namespace Domain;

public interface IOrderRepository: IRepository<Order, OrderId>
{
    
}