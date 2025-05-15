using OrderManagement.Domain;

namespace OrderManagement.Infrastructure.Models;

public class OrderItemReadModel
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; }
    
    private OrderItemReadModel()
    {
    }
    
    public static OrderItemReadModel Create(Guid orderId, OrderItem orderItem)
    {
        return new OrderItemReadModel
        {
            OrderId = orderId,
            ProductId = orderItem.ProductId,
            ProductName = orderItem.ProductName,
            Quantity = orderItem.Quantity,
            Price = orderItem.Price.Amount,
            Currency = orderItem.Price.Currency,
        };
    }
}