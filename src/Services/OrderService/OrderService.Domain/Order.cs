using Core.Domain;

namespace Domain.Orders;

public class Order: AggregateRoot<OrderId>
{
    
    public void AddOrderItem(){}
    
    public void SetAwaitingValidationStatus(){}

    public void SetPaidStatus(){}
    
    public void SetShippedStatus(){}
    
    public void SetCancelledStatus(){}

    public void SetCancelledStatusWhenStockIsRejected(){}
}