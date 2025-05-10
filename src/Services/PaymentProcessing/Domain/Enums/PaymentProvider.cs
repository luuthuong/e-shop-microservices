namespace PaymentProcessing.Domain.Enums;

public enum PaymentProvider
{
    Stripe = 0,
    PayPal = 1,
    BankTransfer = 2,
    CreditCard = 3
}