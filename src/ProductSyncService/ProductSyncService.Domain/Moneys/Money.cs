using Core.Domain;
using Core.Exception;
using ProductSyncService.Domain.Currencies;

namespace ProductSyncService.Domain.Moneys;

public class Money: ValueObject<Money>
{
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    
    private Money(){}

    private Money(decimal amount, string code)
    {
        Amount = amount;
        Currency = Currency.FromCode(code);
    }

    public static Money From(decimal amount, string code)
    {
        if (string.IsNullOrEmpty(code))
            throw new DomainLogicException("Money initialize fail due to code cannot null or empty.");
        if (amount < 0)
            throw new DomainLogicException("Money initialize fail due to value cannot be negative.");
        return new(amount, code);
    }


    public static Money operator *(decimal num, Money right)
    {
        return new(num * right.Amount, right.Currency.Code);
    }

    public static Money operator +(Money money, Money other)
    {
        if (!money.Currency.Code.Equals(other.Currency.Code))
            throw new DomainLogicException("Can not sum with different currencies.");
        
        return From(money.Amount + other.Amount, money.Currency.Code);
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