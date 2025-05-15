namespace PaymentProcessing.Infrastructure.Services;

public interface IPaymentProcessorService
{
    Task<PaymentProcessingResult> ProcessPaymentAsync(
        Guid paymentId,
        decimal amount,
        string currency,
        string paymentMethod,
        string customerId);
}

public class PaymentProcessingResult
{
    public bool Success { get; set; }
    public string TransactionId { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorCode { get; set; }
}