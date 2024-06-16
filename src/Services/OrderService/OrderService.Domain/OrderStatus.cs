namespace Domain.Orders;

public enum OrderStatus
{
    Canceled = 0,
    Placed,
    Processed,
    Paid,
    Shipped,
    Completed
}