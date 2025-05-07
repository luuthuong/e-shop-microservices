namespace PaymentProcessing.Domain;

public enum PaymentStatus
{
    Pending,
    Succeeded,
    Failed,
    Refunded
}