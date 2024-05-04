using Core.Domain;
using Core.Exception;

namespace Domain;

public class CreditLimit : ValueObject<CreditLimit>
{
    public decimal Amount { get; private set; }

    private CreditLimit()
    {
        
    }

    public static CreditLimit From(decimal creditLimit)
    {
        if(creditLimit <= 0) 
            throw new DomainRuleException("The customer credit limit must be greater than zero.");

        return new(creditLimit);
    }


    private CreditLimit(decimal creditLimit)
    {
        Amount = creditLimit;        
    }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Amount;
        }
    }
}