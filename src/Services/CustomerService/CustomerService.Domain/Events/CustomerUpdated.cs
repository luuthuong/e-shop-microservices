using Core.Domain;

namespace Domain.Events;

public record CustomerUpdated : IDomainEvent
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public Address? Address { get; private set; }
    
    public CreditLimit? CreditLimit { get; private set; }
    

    public static CustomerUpdated Create(
        Guid customerId,
        string name, 
        Address? address,
        CreditLimit? creditLimit)
    {        
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));        

        return new CustomerUpdated(
            customerId,
            name,
            address,
            creditLimit
        );
    }

    private CustomerUpdated(
        Guid customerId,
        string name,
        Address? address,
        CreditLimit? creditLimit)
    {
        CustomerId = customerId;
        Name = name;
        Address = address;
        CreditLimit = creditLimit;
    }
}