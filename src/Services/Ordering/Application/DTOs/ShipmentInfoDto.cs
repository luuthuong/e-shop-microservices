using Ordering.Infrastructure.Models;

namespace Ordering.Application.DTOs;

public class ShipmentInfoDto
{
    public string Status { get; set; }
    public string TrackingNumber { get; set; }
    public string Carrier { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }

    public static ShipmentInfoDto? FromShipment(ShipmentReadModel? shipment)
    {
        if (shipment is null)
        {
            return null;
        }
        
        return new ShipmentInfoDto()
        {
            Status = shipment?.Status ?? string.Empty,
            TrackingNumber = shipment?.TrackingNumber ?? string.Empty,
            Carrier = shipment?.Carrier ?? string.Empty,
            ShippedDate = shipment?.ShippedDate,
            DeliveredDate = shipment?.DeliveredDate
        };
    }
}