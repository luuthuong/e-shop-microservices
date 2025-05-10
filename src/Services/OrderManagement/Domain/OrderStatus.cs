namespace OrderManagement.Domain;

public enum OrderStatus
{
    Draft,
    Pending,
    PaymentConfirmed,
    StockReserved,
    OutOfStock,
    ShippingScheduled,
    Shipped,
    Delivered,
    Cancelled
}