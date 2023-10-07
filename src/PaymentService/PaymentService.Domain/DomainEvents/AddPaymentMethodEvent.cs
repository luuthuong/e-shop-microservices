using Core.BaseDomain;

namespace Domain.DomainEvents;

public record AddPaymentMethodEvent(Guid PaymentMethodId): IDomainEvent;