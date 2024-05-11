using Core.Domain;
using Core.Exception;
using Domain.Events;

namespace Domain;

public class Customer: AggregateRoot<CustomerId>
{
    public string Name { get; private set; }
    
    public string  Email { get; private set; } = string.Empty;
    public Address Address { get; private set; }
    
    public CreditLimit CreditLimit { get; private set; }

    private Customer()
    {
        // Only for EF
    }

    public static Customer Create(CustomerData customerData)
    {
        var (email, name, address, creditLimit) = customerData
            ?? throw new ArgumentNullException(nameof(customerData));

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainLogicException("Customer email cannot be null or whitespace.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainLogicException("Customer name cannot be null or whitespace.");
        
        return new (customerData);
    }

    public void Update(CustomerData customerData)
    {
        var (_, name, address, creditLimit) = customerData ?? throw new ArgumentNullException(nameof(customerData));

        if (string.IsNullOrWhiteSpace(customerData.Name))
            throw new DomainLogicException("Customer name cannot be null or whitespace.");

        var @event = CustomerUpdated.Create(
            Id.Value,
            name,
            address,
            creditLimit
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
                      customerData.Address?.StreetAddress,
                      customerData.CreditLimit?.Amount
                  );
        
        RaiseDomainEvent(@event);
        Apply(@event);
    }
    
    private void Apply(CustomerCreated registered)
    {
        Id = CustomerId.From(registered.CustomerId);
        Email = registered.Email;
        Name = registered.Name;
        Address = Address.From(registered.Address ?? string.Empty);
        CreditLimit = CreditLimit.From(registered.CreditLimit ?? 0);
        CreatedDate = DateTime.Now;
    }

    private void Apply(CustomerUpdated updated)
    {
        Name = updated.Name;
        Address = Address.From(updated.Address?.StreetAddress ?? string.Empty);
    }

}