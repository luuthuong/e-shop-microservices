namespace Domain.Entities;

public class Product: BaseEntity
{
    public string Name { get; private set; }
    public long Count { get; private set; }
    public Price Price { get; private set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public Product()
    {
        
    }
    private Product(string name, long count, Price price, Guid categoryId ) =>
        (Name, Count, Price, CategoryId) = (name, count, price, categoryId);
    public static Product Create(string name , long count, Price price, Guid categoryId )
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        return new Product(name, count, price, categoryId)
        {
            CreatedDate = DateTime.Now,
            UpdatedDate = null,
            Id = Guid.NewGuid()
        };
    }
}


public record Price
{
    public string Concurrency { get; init; }
    public long Value { get; init; }

    private Price(string concurrency, long value) => (Concurrency, Value) = (concurrency, value);

    public static Price? Create(string concurrency, long value)
    {
        if (string.IsNullOrEmpty(concurrency))
            return null;
        return new Price(concurrency, value);
    }
}