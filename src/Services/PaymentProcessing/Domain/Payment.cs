using Core.Domain;
using PaymentProcessing.Domain.Events;

namespace PaymentProcessing.Domain;

public class Payment : AggregateRoot
{
    public Guid OrderId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string TransactionId { get; private set; }
    public PaymentMethod Method { get; private set; }
    public DateTime RequestDate { get; private set; }
    public DateTime? ProcessedDate { get; private set; }

    // Required for deserialization
    public Payment()
    {
    }

    public static Payment Create(Guid id, Guid orderId, Money amount, PaymentMethod method)
    {
        var payment = new Payment();

        payment.RaiseEvent(new PaymentCreatedEvent(
            id,
            0,
            orderId,
            amount,
            method,
            DateTime.UtcNow
        ));

        return payment;
    }

    public void Process(string transactionId)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("PaymentProcessing already processed");

        RaiseEvent(new PaymentProcessedEvent(Id, NextVersion, transactionId, DateTime.UtcNow));
    }

    public void MarkAsFailed(string failureReason)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("PaymentProcessing already processed");

        RaiseEvent(new PaymentFailedEvent(Id, NextVersion, failureReason, DateTime.UtcNow));
    }

    public void Refund(string refundReason)
    {
        if (Status != PaymentStatus.Succeeded)
            throw new InvalidOperationException("Only successful payments can be refunded");

        RaiseEvent(new PaymentRefundedEvent(Id, NextVersion, refundReason, DateTime.UtcNow));
    }

    // Apply methods for each event type
    private void Apply(PaymentCreatedEvent @event)
    {
        Id = @event.AggregateId;
        OrderId = @event.OrderId;
        Amount = @event.Amount;
        Method = @event.Method;
        RequestDate = @event.RequestDate;
        Status = PaymentStatus.Pending;
    }

    private void Apply(PaymentProcessedEvent @event)
    {
        Status = PaymentStatus.Succeeded;
        TransactionId = @event.TransactionId;
        ProcessedDate = @event.ProcessedDate;
    }

    private void Apply(PaymentFailedEvent @event)
    {
        Status = PaymentStatus.Failed;
        ProcessedDate = @event.ProcessedDate;
    }

    private void Apply(PaymentRefundedEvent @event)
    {
        Status = PaymentStatus.Refunded;
    }
}