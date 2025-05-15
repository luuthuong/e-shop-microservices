using Core.Domain;
using Core.Exception;

namespace ProductCatalog.Domain.ValueObjects;

public class Price: ValueObject<Price>
{
    public decimal Amount { get; private set; }

    public Price(decimal amount)
    {
        if (amount < 0)
            throw new DomainRuleException("Price cannot be negative");

        Amount = amount;
    }

    protected override IEnumerable<object> EqualityComponents =>
    [
        Amount
    ];
}