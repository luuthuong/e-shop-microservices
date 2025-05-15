using Core.Infrastructure.EF.Projections;
using PaymentProcessing.Domain.Events;
using PaymentProcessing.Infrastructure.Models;

namespace PaymentProcessing.Infrastructure.Projections;

sealed class PaymentDetailsProjection : StreamProjection<PaymentReadModel, PaymentReadDbContext>
{
    public PaymentDetailsProjection(PaymentReadDbContext dbContext) : base(dbContext)
    {
        ProjectEvent<PaymentInitiatedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<PaymentProcessedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<PaymentFailedEvent>((@event, item) => item.Apply(@event));
        ProjectEvent<PaymentRefundedEvent>((@event, item) => item.Apply(@event));
    }
}