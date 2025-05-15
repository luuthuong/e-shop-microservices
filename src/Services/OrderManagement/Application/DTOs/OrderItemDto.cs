using OrderManagement.Domain;
using OrderManagement.Infrastructure.Models;

namespace OrderManagement.Application.DTOs;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public static OrderItemDto FromOrderItem(OrderItemReadModel orderItem)
    {
        return new OrderItemDto()
        {
            ProductId = orderItem.ProductId,
            ProductName = orderItem.ProductName,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.Price,
            TotalPrice = orderItem.Price * orderItem.Quantity
        };
    }
}