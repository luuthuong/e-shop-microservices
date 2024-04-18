using Core.Domain;

namespace Domain.Payments.Events;

public class PaymentCancelled(
    Guid PaymentId,
    PaymentCancelledReason Reason
) : IDomainEvent;
