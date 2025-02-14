using Core.Domain;
using Core.Exception;

namespace Domain;

public class Money : ValueObject<Money>
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public static Money Of(decimal value, string currencyCode)
    {
        if (string.IsNullOrEmpty(currencyCode))
            throw new DomainRuleException("Money must have currency.");

        if (value < 0)
            throw new DomainRuleException("Money amount value cannot be negative.");

        return new Money(value, currencyCode);
    }

    public static Money operator *(decimal number, Money rightValue) => new Money(number * rightValue.Amount, rightValue.Currency.Code);

    public static Money operator +(Money money, Money other)
    {
        if (!money.Currency.Code.Equals(other.Currency.Code))
            throw new DomainRuleException("You cannot sum different currencies.");

        return Of(money.Amount + other.Amount, money.Currency.Code);
    }


    private Money(decimal amount, string currencyCode)
    {
        Amount = amount;
        Currency = Currency.OfCode(currencyCode);
    }

    private Money() {}

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Amount;
            yield return Currency;
        }
    }
}