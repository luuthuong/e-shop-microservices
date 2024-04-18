using Core.Domain;
using Core.Exception;
using Domain.Events;

namespace Domain.Customers;

public class Customer: AggregateRoot<CustomerId>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Address ShippingAddress { get; private set; }

    private Customer()
    {
        // Only for EF
    }


    public static Customer Create(CustomerData customerData)
    {
        var (Email, Name, ShippingAddress) = customerData
            ?? throw new ArgumentNullException(nameof(customerData));

        if (string.IsNullOrWhiteSpace(Email))
            throw new DomainLogicException("Customer email cannot be null or whitespace.");

        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainLogicException("Customer name cannot be null or whitespace.");

        if (string.IsNullOrWhiteSpace(ShippingAddress))
            throw new DomainLogicException("Customer shipping address cannot be null or whitespace.");

        return new (customerData);
    }

    public void UpdateCustomerInformation(CustomerData customerData)
    {
        var (_, name, shippingAddress) = customerData ?? throw new ArgumentNullException(nameof(customerData));

        if (string.IsNullOrWhiteSpace(customerData.Name))
            throw new DomainLogicException("Customer name cannot be null or whitespace.");

        if (string.IsNullOrWhiteSpace(shippingAddress))
            throw new DomainLogicException("Customer shipping address cannot be null or whitespace.");

        var @event = CustomerUpdated.Create(
            Id.Value,
            name,
            shippingAddress
        );
        
        RaiseDomainEvent(@event);
        Apply(@event);
    }

    private Customer(CustomerData customerData)
    {
        var @event = CustomerCreated.Create(
            Guid.NewGuid(),
            customerData.Name,
            customerData.Email,
            customerData.ShippingAddress);
        
        RaiseDomainEvent(@event);
        Apply(@event);
    }
    
    private void Apply(CustomerCreated registered)
    {
        Id = CustomerId.From(registered.CustomerId);
        Email = registered.Email;
        Name = registered.Name;
        ShippingAddress = Address.Create(registered.ShippingAddress);
    }

    private void Apply(CustomerUpdated updated)
    {
        Name = updated.Name;
        ShippingAddress = Address.Create(updated.ShippingAddress);
    }

}