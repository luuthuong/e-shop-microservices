namespace ProductSyncService.Domain.Currencies;

public interface ICurrencyConverter
{
    Currency GetBaseCurrency();
    decimal Covert(decimal value, string code);
}