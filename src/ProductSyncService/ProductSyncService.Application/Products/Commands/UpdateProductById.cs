using Core.CQRS.Command;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Moneys;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Application.Products.Commands;

public sealed record UpdateProductById: ICommand
{
   private UpdateProductById(
      Guid productId, 
      Guid? categoryId, 
      string? name, 
      string? description, 
      string? shortDescription, 
      Money? price, 
      CancellationToken cancellationToken
      )
   {
      ProductId = productId;
      CategoryId = categoryId;
      Name = name;
      Description = description;
      ShortDescription = shortDescription;
      Price = price;
      CancellationToken = cancellationToken;
   }

   public Guid ProductId { get; private set; }
   public  Guid? CategoryId { get;  private set; }
   public string? Name { get; private set; }
   public string? Description { get; private set; }
   public string? ShortDescription { get; private set; }
   public Money?  Price { get; private set; } 
   public CancellationToken CancellationToken { get; private set; }

   public static UpdateProductById Create(
      Guid productId,
      Guid? categoryId,
      string? name,
      string? description,
      string? shortDescription,
      Money? price,
      CancellationToken cancellationToken = default
   ) => new(productId, categoryId, name, description, shortDescription, price, cancellationToken);
}