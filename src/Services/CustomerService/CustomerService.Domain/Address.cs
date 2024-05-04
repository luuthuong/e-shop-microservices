using Core.Domain;

namespace Domain;

public class Address: ValueObject<Address>
{
    public string StreetAddress { get; private set; }

    private Address()
    {
        
    }

    public static Address From(string address)
    {
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