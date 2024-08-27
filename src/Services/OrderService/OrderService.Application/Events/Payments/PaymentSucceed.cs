using Core.Infrastructure.Domain;

namespace Application.Events.Payments;

public sealed class PaymentSucceed: IntegrationDomainEvent
{
    public Guid PaymentId { get; set; }
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public required string CurrencyCode { get; set; }
    public DateTime CompletedAt { get; set; }
}