using Core.Domain;

namespace Application.Shipments.ProcessingShipment.IntegrationEvents;

public class ShipmentFinalized : IIntegrationDomainEvent
{
    public Guid ShipmentId { get; set; }
    public Guid OrderId { get; set; }
    public DateTime ShippedAt { get; set; }
    public Guid Id { get; }
}