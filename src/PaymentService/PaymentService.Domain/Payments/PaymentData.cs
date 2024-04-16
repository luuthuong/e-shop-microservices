namespace Domain.Payments;

public sealed record PaymentData(
    CustomerId CustomerId,
    OrderId OrderId,
    Money TotalAmount
    );