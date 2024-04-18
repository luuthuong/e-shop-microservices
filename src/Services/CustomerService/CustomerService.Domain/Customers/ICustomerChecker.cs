namespace Domain.Customers;

public interface ICustomerChecker
{
    bool IsUniqueEmail(string email);
}