using Ordering.Infrastructure.Models;

namespace Ordering.Application.DTOs;

public class OrderDetailDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public DateTime OrderDate { get; set; }
    public ShippingAddressDto? ShippingAddress { get; set; }
    public List<OrderItemDto> Items { get; set; } = [];
    public PaymentInfoDto? Payment { get; set; }
    public ShipmentInfoDto? Shipment { get; set; }
    public string? CancellationReason { get; set; }
    public List<OrderHistoryEntryDto> History { get; set; } = [];

    public static OrderDetailDto FromOrder(OrderReadModel order, PaymentReadModel? payment = null,
        ShipmentReadModel? shipment = null)
    {
        return new OrderDetailDto()
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Currency = order.Currency,
            OrderDate = order.OrderDate,
            CustomerEmail = order.Metadata.FirstOrDefault(m => m.Key == "CustomerEmail")?.Value,
            CustomerPhone = order.Metadata.FirstOrDefault(m => m.Key == "CustomerPhone")?.Value,
            CancellationReason = order.Metadata.FirstOrDefault(m => m.Key == "CancellationReason")?.Value,
            Items = order.Items.Select(OrderItemDto.FromOrderItem).ToList(),
            History = order.History.Select(OrderHistoryEntryDto.FromOrderHistory).ToList(),
            ShippingAddress = ShippingAddressDto.FromShippingAddress(order.ShippingAddress),
            Payment = payment is not null ? PaymentInfoDto.FromPayment(payment) : null,
            Shipment = ShipmentInfoDto.FromShipment(shipment)
        };
    }
}