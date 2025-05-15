using Core.Domain;

namespace OrderManagement.Domain;

public class Money : ValueObject<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
        
    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
                
        Amount = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Amount;
            yield return Currency;
        }
    }
}