using Core.CQRS.Command;
using ProductSyncService.Domain.Categories;

namespace ProductSyncService.Application.Products.Commands;

public sealed class CreateProduct : ICommand
{
    private CreateProduct(
        string name, 
        Guid categoryId,
        string description, 
        string shortDescription, 
        CancellationToken cancellationToken)
    {
        Name = name;
        CategoryId = CategoryId.From(categoryId);
        ShortDescription = shortDescription;
        CreateProductCancellationToken = cancellationToken;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ShortDescription { get; private set; }
    public CategoryId CategoryId { get; private set; }

    public CancellationToken CreateProductCancellationToken { get; private set; }

    public static CreateProduct Create(
        string name, 
        Guid categoryId,
        string description, 
        string shortDescription,
        CancellationToken cancellationToken = default
        ) => new(name, categoryId, description, shortDescription, cancellationToken);
}