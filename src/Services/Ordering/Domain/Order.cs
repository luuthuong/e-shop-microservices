using Core.Domain;
using Core.Exception;
using Ordering.Domain.Events;

namespace Ordering.Domain;

public class Order : AggregateRoot
{
    public OrderStatus Status { get; private set; }
    public Guid CustomerId { get; private set; }
    public Money TotalAmount { get; private set; }
    public DateTime OrderDate { get; private set; }
    public List<OrderItem> Items { get; private set; } = [];

    public Address ShippingAddress { get; private set; }
    public Dictionary<string, string> Metadata { get; } = new();

    public Order()
    {
    }

    public static Order Create(Guid customerId, IEnumerable<OrderItem> items, Address shippingAddress)
    {
        var order = new Order();
        var orderItems = items.ToList();

        order.RaiseEvent(
            new OrderCreatedEvent(
                order.Id,
                0,
                customerId,
                orderItems,
                CalculateTotalAmount(orderItems),
                DateTime.UtcNow,
                shippingAddress
            )
        );

        return order;
    }

    private static Money CalculateTotalAmount(IEnumerable<OrderItem> items)
    {
        var total = items.Sum(i => i.Quantity * i.Price.Amount);
        return new Money(total, "USD");
    }

    public void SubmitOrder()
    {
        if (Status != OrderStatus.Draft)
            throw new DomainRuleException("Order already submitted");

        RaiseEvent(new OrderSubmittedEvent(Id, NextVersion));
    }

    public void ConfirmPayment()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainRuleException("Order not in pending state");

        RaiseEvent(new OrderPaymentConfirmedEvent(Id, NextVersion));
    }

    public void ReportStockReserved()
    {
        if (Status != OrderStatus.PaymentConfirmed && Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot reserve stock at this stage");

        RaiseEvent(new OrderStockReservedEvent(Id, NextVersion));
    }

    public void ReportOutOfStock()
    {
        if (Status != OrderStatus.Pending && Status != OrderStatus.PaymentConfirmed)
            throw new InvalidOperationException("Cannot report stock issue at this stage");

        RaiseEvent(new OrderOutOfStockEvent(Id, NextVersion));
    }

    public void ScheduleShipping()
    {
        if (Status != OrderStatus.StockReserved)
            throw new DomainRuleException("Order not ready for shipping");

        RaiseEvent(new OrderShippingScheduledEvent(Id, NextVersion));
    }

    public void ReportShipped()
    {
        if (Status != OrderStatus.ShippingScheduled)
            throw new DomainRuleException("Order not scheduled for shipping");

        RaiseEvent(new OrderShippedEvent(Id, NextVersion));
    }

    public void ReportDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new DomainRuleException("Order not shipped yet");

        RaiseEvent(new OrderDeliveredEvent(Id, NextVersion));
    }

    public void CancelOrder(string reason)
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot cancel order in current state");

        RaiseEvent(new OrderCancelledEvent(Id, NextVersion, reason));
    }

    public void AddMetadata(string key, string value)
    {
        RaiseEvent(new OrderMetadataAddedEvent(Id, NextVersion, key, value));
    }

    #region Apply Events

    void Apply(OrderCreatedEvent @event)
    {
        Id = @event.AggregateId;
        CustomerId = @event.CustomerId;
        Items = @event.Items;
        TotalAmount = @event.TotalAmount;
        OrderDate = @event.OrderDate;
        Status = OrderStatus.Draft;
    }

    void Apply(OrderSubmittedEvent @event)
    {
        Status = OrderStatus.Pending;
    }

    void Apply(OrderPaymentConfirmedEvent @event)
    {
        Status = OrderStatus.PaymentConfirmed;
    }

    void Apply(OrderStockReservedEvent @event)
    {
        Status = OrderStatus.StockReserved;
    }

    void Apply(OrderOutOfStockEvent @event)
    {
        Status = OrderStatus.OutOfStock;
    }

    void Apply(OrderShippingScheduledEvent @event)
    {
        Status = OrderStatus.ShippingScheduled;
    }

    void Apply(OrderShippedEvent @event)
    {
        Status = OrderStatus.Shipped;
    }

    void Apply(OrderDeliveredEvent @event)
    {
        Status = OrderStatus.Delivered;
    }

    void Apply(OrderCancelledEvent @event)
    {
        Status = OrderStatus.Cancelled;
    }

    void Apply(ShippingAddressSetEvent @event)
    {
        ShippingAddress = @event.Address;
    }

    void Apply(OrderMetadataAddedEvent @event)
    {
        Metadata[@event.Key] = @event.Value;
    }

    #endregion
}