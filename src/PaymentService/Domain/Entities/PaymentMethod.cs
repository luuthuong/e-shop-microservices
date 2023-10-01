using Core.BaseDomain;
using Domain.DomainEvents;

namespace Domain.Entities;

public class PaymentMethod: AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public PaymentMethod(){}

    private PaymentMethod(string name, string description = "") =>
        (Id, Name, Description, CreatedDate) = (Guid.NewGuid(), name, description, DateTime.Now);

    public static PaymentMethod Create(string name, string description = "")
    {
        var paymentMethod = new PaymentMethod(name, description);
        paymentMethod.RaiseDomainEvent(new AddPaymentMethodEvent(paymentMethod.Id));
        return paymentMethod;
    }
}