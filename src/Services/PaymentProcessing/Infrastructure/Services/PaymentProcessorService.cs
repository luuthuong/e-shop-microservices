namespace PaymentProcessing.Infrastructure.Services;

public class PaymentProcessorService(ILogger<PaymentProcessorService> logger) : IPaymentProcessorService
{
    private readonly ILogger<PaymentProcessorService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<PaymentProcessingResult> ProcessPaymentAsync(
        Guid paymentId,
        decimal amount,
        string currency,
        string paymentMethod,
        string customerId)
    {
        _logger.LogInformation(
            "Processing payment {PaymentId} for {Amount} {Currency} using {PaymentMethod}",
            paymentId,
            amount,
            currency,
            paymentMethod);

        try
        {
            await Task.Delay(500);

            // Simulate a successful payment most of the time
            if (new Random().Next(10) < 8) // 80% success rate
            {
                var transactionId = $"tx_{Guid.NewGuid():N}";

                _logger.LogInformation(
                    "Payment {PaymentId} processed successfully with transaction ID {TransactionId}",
                    paymentId,
                    transactionId);

                return new PaymentProcessingResult
                {
                    Success = true,
                    TransactionId = transactionId
                };
            }
            else
            {
                // Simulate various failure scenarios
                var errorCode = new Random().Next(4) switch
                {
                    0 => "INSUFFICIENT_FUNDS",
                    1 => "CARD_DECLINED",
                    2 => "PAYMENT_GATEWAY_ERROR",
                    _ => "INVALID_CARD"
                };

                var errorMessage = errorCode switch
                {
                    "INSUFFICIENT_FUNDS" => "Insufficient funds in the account",
                    "CARD_DECLINED" => "Card was declined by the issuer",
                    "PAYMENT_GATEWAY_ERROR" => "Payment gateway is experiencing technical issues",
                    "INVALID_CARD" => "Card information is invalid or expired",
                    _ => "Unknown error occurred"
                };

                _logger.LogWarning(
                    "Payment {PaymentId} failed: {ErrorMessage} ({ErrorCode})",
                    paymentId,
                    errorMessage,
                    errorCode);

                return new PaymentProcessingResult
                {
                    Success = false,
                    ErrorMessage = errorMessage,
                    ErrorCode = errorCode
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception occurred while processing payment {PaymentId}",
                paymentId);

            return new PaymentProcessingResult
            {
                Success = false,
                ErrorMessage = "An unexpected error occurred while processing the payment",
                ErrorCode = "SYSTEM_ERROR"
            };
        }
    }
}