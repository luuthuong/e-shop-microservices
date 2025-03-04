using Core.Domain;

namespace Application.Shipments.ProcessingShipment.IntegrationEvents;

public class ProductWasOutOfStock(Guid orderId) : IIntegrationDomainEvent
{
    public Guid OrderId { get; private set; } = orderId;
    public DateTime CheckedAt { get; private set; } = DateTime.UtcNow;
    public Guid Id { get; }
}