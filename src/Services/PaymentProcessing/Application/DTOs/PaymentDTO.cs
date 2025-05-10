using PaymentProcessing.Infrastructure.Models;

namespace PaymentProcessing.Application.DTOs;

public class PaymentDTO
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public string PaymentMethod { get; set; }
    public string Provider { get; set; }
    public string TransactionId { get; set; }
    public string FailureReason { get; set; }
    public string ErrorCode { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string RefundId { get; set; }
    public decimal? RefundAmount { get; set; }
    public string RefundReason { get; set; }
    public DateTime? RefundDate { get; set; }

    public static PaymentDTO From(PaymentReadModel model)
    {
        if (model == null)
            return null;

        return new PaymentDTO
        {
            Id = model.Id,
            OrderId = model.OrderId,
            CustomerId = model.CustomerId,
            Amount = model.Amount,
            Currency = model.Currency,
            Status = model.Status.ToString(),
            PaymentMethod = model.PaymentMethod,
            Provider = model.Provider.ToString(),
            TransactionId = model.TransactionId,
            FailureReason = model.FailureReason,
            ErrorCode = model.ErrorCode,
            ProcessedDate = model.ProcessedDate,
            RefundId = model.RefundId,
            RefundAmount = model.RefundAmount,
            RefundReason = model.RefundReason,
            RefundDate = model.RefundDate
        };
    }
}