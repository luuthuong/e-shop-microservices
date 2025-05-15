using Core.Domain;
using OrderManagement.Domain;
using OrderManagement.Domain.Events;

namespace OrderManagement.Infrastructure.Models;

public sealed class OrderReadModel: BaseEntity
{
    public Guid CustomerId { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public DateTime LastUpdated { get; private set; }
    public int Version { get; private set; }
    public ICollection<OrderItemReadModel> Items { get; private set; } = [];
    public OrderShippingAddressReadModel? ShippingAddress { get; private set; }
    public ICollection<OrderMetadataReadModel> Metadata { get; private set; } = [];
    public ICollection<OrderHistoryReadModel> History { get; private set; } = [];

    internal void Apply(OrderCreatedEvent @event)
    {
        Id = @event.AggregateId;
        CustomerId = @event.CustomerId;
        Status = OrderStatus.Draft.ToString();
        TotalAmount = @event.TotalAmount.Amount;
        Currency = @event.TotalAmount.Currency;
        OrderDate = @event.OrderDate;
        LastUpdated = DateTime.UtcNow;
        Version = @event.Version;
        Items = @event.Items.Select(i => OrderItemReadModel.Create(@event.AggregateId, i)).ToList();
        ShippingAddress = OrderShippingAddressReadModel.Create(@event.AggregateId, @event.ShippingAddress);
        History.Add(OrderHistoryReadModel.Create(@event.AggregateId, Status, "Order Created", @event.Timestamp));
    }

    internal void Apply(OrderSubmittedEvent @event)
    {
        Status = OrderStatus.Pending.ToString();
        LastUpdated = DateTime.UtcNow;
        Version = @event.Version;
        History.Add(OrderHistoryReadModel.Create(@event.AggregateId, Status, "Order Submitted", @event.Timestamp));
    }
}