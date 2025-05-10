using Core.Domain;
using Core.Exception;
using PaymentProcessing.Domain.Enums;
using PaymentProcessing.Domain.Events;

namespace PaymentProcessing.Domain;

public class PaymentAggregate : AggregateRoot
{
    // Properties
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

    public PaymentAggregate()
    {
    }

    // Constructor for creating a new payment
    private PaymentAggregate(
        Guid orderId,
        string customerId,
        decimal amount,
        string currency,
        string paymentMethod)
    {
        RaiseEvent(new PaymentInitiatedEvent(
            Id,
            NextVersion,
            orderId,
            customerId,
            amount,
            currency,
            paymentMethod));
    }

    public static PaymentAggregate Create(
        Guid orderId,
        string customerId,
        decimal amount,
        string currency,
        string paymentMethod)
    {
        if (orderId == Guid.Empty)
            throw new DomainRuleException("Order ID cannot be empty");

        if (string.IsNullOrWhiteSpace(customerId))
            throw new DomainRuleException("Customer ID cannot be empty");

        if (amount <= 0)
            throw new DomainRuleException("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainRuleException("Currency cannot be empty");

        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new DomainRuleException("Payment method cannot be empty");

        return new PaymentAggregate(orderId, customerId, amount, currency, paymentMethod);
    }

    public void ProcessPayment(string transactionId)
    {
        if (Status != PaymentStatus.Pending && Status != PaymentStatus.Processing)
            throw new DomainRuleException($"Cannot process payment in state {Status}");

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new DomainRuleException("Transaction ID cannot be empty");

        RaiseEvent(new PaymentProcessedEvent(
            Id,
            NextVersion,
            transactionId,
            DateTime.UtcNow));
    }

    // Mark a payment as failed
    public void FailPayment(string reason, string errorCode)
    {
        if (Status != PaymentStatus.Pending && Status != PaymentStatus.Processing)
            throw new DomainRuleException($"Cannot fail payment in state {Status}");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainRuleException("Failure reason cannot be empty");

        RaiseEvent(new PaymentFailedEvent(
            Id,
            NextVersion,
            reason,
            errorCode,
            DateTime.UtcNow));
    }

    // Refund a payment
    public void RefundPayment(string refundId, decimal refundAmount, string reason)
    {
        if (Status != PaymentStatus.Completed)
            throw new DomainRuleException($"Cannot refund payment in state {Status}");

        if (string.IsNullOrWhiteSpace(refundId))
            throw new DomainRuleException("Refund ID cannot be empty");

        if (refundAmount <= 0 || refundAmount > Amount)
            throw new DomainRuleException(
                "Refund amount must be greater than zero and less than or equal to the payment amount");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainRuleException("Refund reason cannot be empty");

        RaiseEvent(new PaymentRefundedEvent(
            Id,
            NextVersion,
            refundId,
            refundAmount,
            reason,
            DateTime.UtcNow));
    }

    // Event application methods
    private void Apply(PaymentInitiatedEvent @event)
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

    private void Apply(PaymentProcessedEvent @event)
    {
        TransactionId = @event.TransactionId;
        ProcessedDate = @event.ProcessedDate;
        Status = PaymentStatus.Completed;
    }

    private void Apply(PaymentFailedEvent @event)
    {
        FailureReason = @event.FailureReason;
        ErrorCode = @event.ErrorCode;
        Status = PaymentStatus.Failed;
    }

    private void Apply(PaymentRefundedEvent @event)
    {
        RefundId = @event.RefundId;
        RefundAmount = @event.RefundAmount;
        RefundReason = @event.RefundReason;
        RefundDate = @event.RefundDate;
        Status = PaymentStatus.Refunded;
    }

    // Helper methods
    private PaymentProvider MapPaymentMethodToProvider(string paymentMethod)
    {
        return paymentMethod.ToLowerInvariant() switch
        {
            "stripe" => PaymentProvider.Stripe,
            "paypal" => PaymentProvider.PayPal,
            "bank_transfer" => PaymentProvider.BankTransfer,
            "credit_card" => PaymentProvider.CreditCard,
            _ => throw new DomainRuleException($"Unsupported payment method: {paymentMethod}")
        };
    }
}