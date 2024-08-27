using Core.Domain;

namespace Domain.Orders;

public class Order : AggregateRoot<OrderId>
{
    public CustomerId CustomerId { get; private set; }
    public PaymentId PaymentId { get; private set; }
    public Money TotalPrice{ get; private set; }
    public OrderStatus Status { get; private set; }
    
    public void AddOrderItem() { }

    public void SetAwaitingValidationStatus() { }

    public void SetPaidStatus() { }

    public void SetShippedStatus() { }

    public void SetCancelledStatus() { }

    public void SetCancelledStatusWhenStockIsRejected() { }
}
