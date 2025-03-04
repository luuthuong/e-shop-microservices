using Application.Payments.ProcessingPayment.IntegrationEvents;
using Application.Shipments.ProcessingShipment.IntegrationEvents;
using Core.CQRS.Command;
using Core.EventBus;
using Domain;
using Domain.Commands;
using Domain.Events;

namespace Application;

/// <summary>
/// Handles compensation events for OrderSaga
/// </summary>
public class OrderSagaCompensation(ICommandBus commandBus) :
    IEventHandler<PaymentFailed>,
    IEventHandler<CustomerReachedCreditLimit>,
    IEventHandler<ShipmentFailed>,
    IEventHandler<ProductWasOutOfStock>,
    IEventHandler<OrderCanceled>    
{
    private readonly ICommandBus _commandBus = commandBus;

    public Task Handle(PaymentFailed @integrationEvent, 
        CancellationToken cancellationToken)
    {
        // Payment failed due to issues
        return default;
    }

    public async Task Handle(CustomerReachedCreditLimit @integrationEvent, 
        CancellationToken cancellationToken)
    {
        // Payment failed due to credit limit
    }

    public Task Handle(ShipmentFailed @integrationEvent, 
        CancellationToken cancellationToken)
    {
        // Shipment failed due to issues
        return default;
    }

    public async Task Handle(ProductWasOutOfStock @integrationEvent, 
        CancellationToken cancellationToken)
    {
        // Product was out of stock when shipping 
        return;
    }

    public async Task Handle(OrderCanceled @integrationEvent, 
        CancellationToken cancellationToken)
    {
        return;
    }
}