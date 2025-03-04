using Core.Domain;

namespace Application.Shipments.ProcessingShipment.IntegrationEvents;

public class ShipmentFailed(
    Guid shippingId,
    Guid orderId) : IIntegrationDomainEvent
{
    public Guid ShippingId { get; } = shippingId;
    public Guid OrderId { get; } = orderId;
    public DateTime FailedAt { get; } = DateTime.UtcNow;
    public Guid Id { get; }
}
