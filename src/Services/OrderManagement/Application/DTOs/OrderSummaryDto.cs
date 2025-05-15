using OrderManagement.Infrastructure.Models;

namespace OrderManagement.Application.DTOs;

public class OrderSummaryDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerEmail { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public DateTime OrderDate { get; set; }
    public int ItemCount { get; set; }

    public static OrderSummaryDto From(OrderReadModel order, string customerEmail, int itemCount)
    {
        return new OrderSummaryDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerEmail = customerEmail,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Currency = order.Currency,
            OrderDate = order.OrderDate,
            ItemCount = itemCount
        };
    }
    
}