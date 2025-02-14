using Core.Domain;
using Core.Exception;

namespace Domain;

public class Currency : ValueObject<Currency>
{
    public string Code { get; }
    public string Symbol { get; }
    public static Currency USDollar => new Currency("USD", "$");
    public static Currency CanadianDollar => new Currency("CAD", "CA$");
    public static Currency Euro => new Currency("EUR", "€");

    public static Currency OfCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainRuleException("Code cannot be null or whitespace.");

        return code switch
        {
            "USD" => new Currency(USDollar.Code, USDollar.Symbol),
            "CAD" => new Currency(CanadianDollar.Code, CanadianDollar.Symbol),
            "EUR" => new Currency(Euro.Code, Euro.Symbol),
            _ => throw new DomainRuleException($"Invalid code {code}")
        };
    }

    private Currency(string code, string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new DomainRuleException("Symbol cannot be null or whitespace.");

        Code = code;
        Symbol = symbol;
    }

    private Currency() { }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Code;
            yield return Symbol;
        }
    }
}