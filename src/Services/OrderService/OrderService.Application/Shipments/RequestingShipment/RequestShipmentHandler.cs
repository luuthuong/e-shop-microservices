using Core.CQRS.Command;

namespace Application.Shipments.RequestingShipment;

public class RequestShipmentHandler
     : ICommandHandler<RequestShipment>
{

    public async Task Handle(RequestShipment command, CancellationToken cancellationToken)
    {
    }
}

public record class ShipOrderRequest(
    Guid OrderId,
    IReadOnlyList<ProductItemRequest> ProductItems);

public record ProductItemRequest(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);