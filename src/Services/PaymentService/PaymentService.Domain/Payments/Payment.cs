using Core.Domain;
using Core.Exception;
using Domain.Payments.Events;

namespace Domain.Payments;

public class Payment : AggregateRoot<PaymentId>
{
    public CustomerId CustomerId { get; private set; }
    public OrderId OrderId { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }
    public Money TotalAmount { get; private set; }

    public static Payment Create(PaymentData paymentData)
    {
        var (customerId, orderId, totalAmount) = paymentData
                                                 ?? throw new ArgumentNullException(nameof(paymentData));

        if (customerId is null)
            throw new DomainRuleException("The customer Id is required.");

        if (orderId is null)
            throw new DomainRuleException("The order Id is required.");

        if (totalAmount is null)
            throw new DomainRuleException("The total amount is required.");

        return new Payment(paymentData);
    }
    
    public void Complete()
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainLogicException($"Cannot be completed while current status is '{Status}'");

        var @event = new PaymentCompleted(Id.Value);

        RaiseDomainEvent(@event);
        Apply(@event);
    }

    public void Cancelled(PaymentCancelledReason reason)
    {
        if (Status == PaymentStatus.Canceled)
            throw new DomainLogicException($"Cannot be canceled while current status is '{Status}'");

        var @event = new PaymentCancelled(
            Id.Value,
            reason
        );

        RaiseDomainEvent(@event);
        Apply(@event);
    }

    private Payment(PaymentData paymentData)
    {
        var @event = new PaymentCreated(
            Guid.NewGuid(),
            paymentData.CustomerId.Value,
            paymentData.OrderId.Value,
            paymentData.TotalAmount.Amount,
            paymentData.TotalAmount.Currency.Code
        );

        RaiseDomainEvent(@event);
        Apply(@event);
    }


    private void Apply(PaymentCreated @event)
    {
        Status = PaymentStatus.Pending;
        Id = PaymentId.From(@event.PaymentId);
        CustomerId = CustomerId.From(@event.CustomerId);
        OrderId = OrderId.From(@event.OrderId);
        TotalAmount = Money.From(
            @event.TotalAmount,
            @event.CurrencyCode
        );
        CreatedAt = DateTime.Now;
    }

    private void Apply(PaymentCompleted @event)
    {
        Status = PaymentStatus.Completed;
        CompletedAt = DateTime.Now;
    }

    private void Apply(PaymentCancelled @event)
    {
        Status = PaymentStatus.Canceled;
        CanceledAt = DateTime.Now;
    }
}