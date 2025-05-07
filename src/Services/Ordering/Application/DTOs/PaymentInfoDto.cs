using Ordering.Infrastructure.Models;

namespace Ordering.Application.DTOs;

public class PaymentInfoDto
{
    public Guid PaymentId { get; set; }
    public string Status { get; set; }
    public string TransactionId { get; set; }
    public string Method { get; set; }
    public DateTime? ProcessedDate { get; set; }

    public static PaymentInfoDto? FromPayment(PaymentReadModel? payment)
    {
        if (payment is null)
            return null;
        
        return new PaymentInfoDto()
        {
            PaymentId = payment.Id,
            Status = payment.Status,
            TransactionId = payment.TransactionId,
            Method = payment.Method,
            ProcessedDate = payment.ProcessedDate
        };
    }
}