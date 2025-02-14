using Application.Payments.ProcessingPayment.IntegrationEvents;
using Application.Shipments.ProcessingShipment.IntegrationEvents;
using Core.CQRS.Command;
using Core.EventBus;
using Domain.Events;

namespace Application;

public class OrderSaga(ICommandBus commandBus) :
    IEventHandler<OrderPlaced>,
    IEventHandler<OrderProcessed>,
    IEventHandler<PaymentFinalized>,
    IEventHandler<ShipmentFinalized>
{

    /// <summary>
    /// Processing placed order
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(OrderPlaced @domainEvent,
        CancellationToken cancellationToken)
    {
       
    }

    /// <summary>
    /// Requesting payment
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(OrderProcessed @domainEvent,
        CancellationToken cancellationToken)
    {
       
    }

    /// <summary>
    /// Requesting shipment
    /// </summary>
    /// <param name="integrationEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(PaymentFinalized @integrationEvent,
        CancellationToken cancellationToken)
    {
       
    }

    /// <summary>
    /// Completing order
    /// </summary>
    /// <param name="integrationEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(ShipmentFinalized @integrationEvent,
        CancellationToken cancellationToken)
    {
      
    }

}