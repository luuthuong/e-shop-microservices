using Core.Domain;

namespace Application.Payments.ProcessingPayment.IntegrationEvents;

public class CustomerReachedCreditLimit : IIntegrationDomainEvent
{
    public Guid OrderId { get; private set; }
    public DateTime CheckedAt { get; private set; }

    public CustomerReachedCreditLimit(Guid orderId)
    {
        OrderId = orderId;
        CheckedAt = DateTime.UtcNow;
    }

    public Guid Id { get; }
}