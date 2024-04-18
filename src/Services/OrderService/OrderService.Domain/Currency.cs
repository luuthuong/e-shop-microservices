using Core.Domain;
using Core.Exception;

namespace Domain;

public class Currency: ValueObject<Currency>
{
    public string Code { get; }
    public string Symbol { get; }

    private Currency()
    {
        
    }
    
    private Currency(string code, string symbol)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new DomainRuleException("Symbol can not null or whitespace");
        Code = code;
        Symbol = symbol;
    }

    public static Currency Dollar => new("USD", "$");
    public static Currency Euro => new("EUR", "€");
    public static Currency Vnd => new("VND", "đ");

    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainLogicException("Code can not null or whitespace.");
        return code switch
        {
            "USD" => new(Dollar.Code, Dollar.Symbol),
            "EUR" => new(Euro.Code, Euro.Symbol),
            "VND" => new(Vnd.Code, Vnd.Symbol),
            _ => throw new DomainLogicException($"Invalid code currency {code}")
        };
    }
    
    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Code;
            yield return Symbol;
        }
    }
}