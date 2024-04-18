
namespace API.Requests.Payments;

internal sealed record CreatePaymentRequest(
    Guid CustomerId,
    Guid OrderId,
    decimal TotalAmount,
    string CurrencyCode
);