namespace Ordering.Infrastructure.Models;

public class ShipmentReadModel
{
    public ShipmentReadModel()
    {
    }
    
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string Status { get; private set; }
    public string TrackingNumber { get; private set; }
    public string Carrier { get; private set; }
    public DateTime? ShippedDate { get; private set; }
    public DateTime? DeliveredDate { get; private set; }
}