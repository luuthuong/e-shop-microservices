namespace Ordering.Domain;

public class Address
{
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }
    public required string Country { get; init; }
    public required string ZipCode { get; init; }
    public required string RecipientName { get; init; }
    public required string PhoneNumber { get; init; }
}