using Core.Domain;
using Core.Exception;

namespace ProductCatalog.Domain.ValueObjects;

public class Money : ValueObject<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new DomainRuleException("Money amount cannot be negative");

        if (string.IsNullOrEmpty(currency))
            throw new DomainRuleException("Currency code is required");

        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object> EqualityComponents =>
    [
        Currency
    ];
}