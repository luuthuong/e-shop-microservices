using Core.Domain;

namespace Domain.Payments.Events;

public class PaymentCompleted(
    Guid PaymentId
) : IDomainEvent;
