namespace Domain.Entities;

public class Product: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public long Count { get; private set; }
    
}