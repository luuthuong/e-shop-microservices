namespace ProductSyncService.Domain.Currencies;

public interface ICurrencyConverter
{
    Currency GetBaseCurrency();
    decimal Convert(decimal value, string code);
}