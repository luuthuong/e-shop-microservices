using MediatR;

namespace PaymentProcessing.Application.Commands.ProcessPayment;

public record ProcessPaymentCommand(
    Guid OrderId,
    string CustomerId,
    decimal Amount,
    string Currency,
    string PaymentMethod) : IRequest<ProcessPaymentResult>;

public class ProcessPaymentResult
{
    public bool Success { get; set; }
    public Guid PaymentId { get; set; }
    public string TransactionId { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorCode { get; set; }
}