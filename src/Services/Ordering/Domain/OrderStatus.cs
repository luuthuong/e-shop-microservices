namespace Ordering.Domain;

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