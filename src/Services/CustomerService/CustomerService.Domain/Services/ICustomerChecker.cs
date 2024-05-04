namespace Domain;

public interface ICustomerChecker
{
    bool IsUniqueEmail(string email);
}