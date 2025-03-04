using Core.CQRS.Command;
using Domain.Commands;

namespace Application.Shipments.RecordingShipment;

public class RecordShipmentHandler() : ICommandHandler<RecordShipment>
{
    public async Task Handle(RecordShipment command, CancellationToken cancellationToken)
    {
       
    }
}