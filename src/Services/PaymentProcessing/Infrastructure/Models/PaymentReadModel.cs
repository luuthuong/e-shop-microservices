namespace PaymentProcessing.Infrastructure.Models;

public class PaymentReadModel
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string Status { get; private set; } = null!;
    public string? TransactionId { get; private set; }
    public string Method { get; private set; } = null!;
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
}