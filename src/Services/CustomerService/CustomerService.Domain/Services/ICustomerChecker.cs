namespace Domain;

public interface ICustomerChecker
{
    Task<bool> IsUniqueEmail(string email);
}