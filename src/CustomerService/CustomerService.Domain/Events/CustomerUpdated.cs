using Core.Domain;

namespace Domain.Events;

public record class CustomerUpdated : IDomainEvent
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string ShippingAddress { get; private set; }

    public static CustomerUpdated Create(
        Guid customerId,
        string name, 
        string shippingAddress)
    {        
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));        
        if (string.IsNullOrEmpty(shippingAddress))
            throw new ArgumentNullException(nameof(shippingAddress));

        return new CustomerUpdated(
            customerId, 
            name, 
            shippingAddress
            );
    }

    private CustomerUpdated(
        Guid customerId,
        string name,
        string shippingAddress)
    {
        CustomerId = customerId;
        Name = name;
        ShippingAddress = shippingAddress;
    }
}