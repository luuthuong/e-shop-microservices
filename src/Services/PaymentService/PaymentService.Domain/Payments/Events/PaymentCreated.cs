using Core.Domain;

namespace Domain.Payments.Events;

public record PaymentCreated(
    Guid PaymentId,
    Guid CustomerId,
    Guid OrderId,
    decimal TotalAmount,
    string CurrencyCode
) : IDomainEvent;