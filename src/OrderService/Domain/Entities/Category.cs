using Core.BaseDomain;
using Domain.DomainEvents.Products;

namespace Domain.Entities;

public class Category: AggregateRoot
{
    public string Name { get; set; }

    private readonly List<Product> _products = new();

    public virtual IList<Product> ListProducts { get; private set; }

    private Category(string name)
    {
        Name = name;
        CreatedDate = DateTime.Now;
    }

    public static Category Create(string name)
    {
        return new Category(name);
    }

    public void PublishProduct(Product product)
    {
        product.MarkAsPublish();
        RaiseDomainEvent(new PublishProductDomainEvent(product.Id, Id));
    }
    
    public void UnPublishProduct(Product product)
    {
        product.MarkAsUnPublish();
        RaiseDomainEvent(new UnPublishProductDomainEvent(product.Id, Id));
    }
    
}