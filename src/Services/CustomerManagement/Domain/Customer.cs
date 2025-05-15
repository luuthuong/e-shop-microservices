using Core.Exception;

namespace CustomerManagement.Domain;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    
    private Customer()
    {
    }

    private Customer(Guid id, string name, string email, string phoneNumber)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    
    public static Customer Create(Guid id, string name, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("Name cannot be empty");
        
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainRuleException("Email cannot be empty");
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainRuleException("Phone number cannot be empty");
        
        return new Customer(id, name, email, phoneNumber);
    }

    public void UpdateContactInfo(string email, string phoneNumber)
    {
        Email = email;
        PhoneNumber = phoneNumber;
    }
}