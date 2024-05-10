using Core.Domain;

namespace Domain.Events;

public class CustomerCreated: IDomainEvent
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Address { get; private set; }
    
    public decimal? CreditLimit { get; private set; }

    private CustomerCreated()
    {
    }

    public static CustomerCreated Create(
        Guid customerId,
        string name,
        string email,
        string? shippingAddress,
        decimal? creditLimit)
    {     
        if (customerId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(customerId));
        
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));

        return new(
            customerId, 
            name, 
            email, 
            shippingAddress,
            creditLimit
        );            
    }

    private CustomerCreated(
        Guid customerId,
        string name,
        string email,
        string? shippingAddress,
        decimal? creditLimit
        )
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
        Address = shippingAddress;
        CreditLimit = creditLimit;
    }
}