using PaymentProcessing.Domain.Enums;
using PaymentProcessing.Domain.Events;

namespace PaymentProcessing.Infrastructure.Models;

public class PaymentReadModel
{
    public PaymentReadModel()
    {
    }

    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string CustomerId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string PaymentMethod { get; private set; }
    public PaymentProvider Provider { get; private set; }
    public string TransactionId { get; private set; }
    public string FailureReason { get; private set; }
    public string ErrorCode { get; private set; }
    public DateTime? ProcessedDate { get; private set; }
    public string RefundId { get; private set; }
    public decimal? RefundAmount { get; private set; }
    public string RefundReason { get; private set; }
    public DateTime? RefundDate { get; private set; }

    internal void Apply(PaymentInitiatedEvent @event)
    {
        Id = @event.AggregateId;
        OrderId = @event.OrderId;
        CustomerId = @event.CustomerId;
        Amount = @event.Amount;
        Currency = @event.Currency;
        PaymentMethod = @event.PaymentMethod;
        Status = PaymentStatus.Pending;
        Provider = MapPaymentMethodToProvider(@event.PaymentMethod);
    }

    private PaymentProvider MapPaymentMethodToProvider(string paymentMethod)
    {
        return paymentMethod.ToLowerInvariant() switch
        {
            "stripe" => PaymentProvider.Stripe,
            "paypal" => PaymentProvider.PayPal,
            "bank_transfer" => PaymentProvider.BankTransfer,
            "credit_card" => PaymentProvider.CreditCard,
            _ => throw new Exception($"Unsupported payment method: {paymentMethod}")
        };
    }

    internal void Apply(PaymentProcessedEvent @event)
    {
        TransactionId = @event.TransactionId;
        ProcessedDate = @event.ProcessedDate;
        Status = PaymentStatus.Completed;
    }

    internal void Apply(PaymentFailedEvent @event)
    {
        FailureReason = @event.FailureReason;
        ErrorCode = @event.ErrorCode;
        Status = PaymentStatus.Failed;
    }

    internal void Apply(PaymentRefundedEvent @event)
    {
        RefundId = @event.RefundId;
        RefundAmount = @event.RefundAmount;
        RefundReason = @event.RefundReason;
        RefundDate = @event.RefundDate;
        Status = PaymentStatus.Refunded;
    }
}