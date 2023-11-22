using Core.Domain;

namespace Domain.DomainEvents;

public record AddPaymentMethodEvent(Guid PaymentMethodId): IDomainEvent;