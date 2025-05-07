namespace Ordering.Infrastructure.Models;

public class OrderMetadataReadModel
{
    public OrderMetadataReadModel()
    {
    }

    public Guid OrderId { get; private set; }
    public string Key { get; private set; }
    public string Value { get; private set; }
}