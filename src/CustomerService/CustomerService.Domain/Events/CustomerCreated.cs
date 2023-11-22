using Core.Domain;

namespace Domain.Events;

public class CustomerCreated: IDomainEvent
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string ShippingAddress { get; private set; }

    public static CustomerCreated Create(
        Guid customerId,
        string name,
        string email,
        string shippingAddress)
    {      
        if (customerId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(customerId));
        
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));
        
        if (string.IsNullOrEmpty(shippingAddress))
            throw new ArgumentNullException(nameof(shippingAddress));

        return new(
            customerId, 
            name, 
            email, 
            shippingAddress);            
    }

    private CustomerCreated(
        Guid customerId,
        string name,
        string email,
        string shippingAddress)
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
        ShippingAddress = shippingAddress;
    }
}