namespace API.Requests.Payments;

internal sealed record CancellationPaymentRequest(int PaymentCancelReason);
