using MediatR;

namespace PaymentProcessing.Application.Commands.RefundPayment;

public record RefundPaymentCommand(
    Guid PaymentId,
    decimal RefundAmount,
    string Reason) : IRequest<RefundPaymentResult>;

public class RefundPaymentResult
{
    public bool Success { get; set; }
    public string RefundId { get; set; }
    public string ErrorMessage { get; set; }
}