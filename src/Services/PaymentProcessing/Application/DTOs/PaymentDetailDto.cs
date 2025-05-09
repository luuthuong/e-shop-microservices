using PaymentProcessing.Infrastructure.Models;

namespace PaymentProcessing.Application.DTOs;

public class PaymentDetailDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public DateTime OrderDate { get; set; }
}