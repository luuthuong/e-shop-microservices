using Core.Domain;
using Core.Exception;

namespace Domain;

public class Address: ValueObject<Address>
{
    public string StreetAddress { get; private set; }

    public static Address Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainLogicException("Address cannot be null or whitespace.");
        return new Address(address);
    }

    private Address(string address)
    {
        StreetAddress = address;        
    }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return StreetAddress;
        }
    }
}