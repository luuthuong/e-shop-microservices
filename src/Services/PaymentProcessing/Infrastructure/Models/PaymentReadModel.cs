namespace PaymentProcessing.Infrastructure.Models;

public class PaymentReadModel
{
    public PaymentReadModel()
    {
    }
    
    public Guid Id { get; set; }
    public Guid OrderId { get; private set; }
    public string Status { get; private set; }
    public string TransactionId { get; private set; }
    public string Method { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public DateTime? ProcessedDate { get; private set; }
}