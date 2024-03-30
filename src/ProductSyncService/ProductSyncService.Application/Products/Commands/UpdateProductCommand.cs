using Core.CQRS.Command;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Moneys;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Application.Products.Commands;

public sealed record UpdateProductCommand(
    ProductId ProductId,
    CategoryId? CategoryId,
    string Name,
    string? Description,
    string ShortDescription = "",
    Money? Price = null
) : ICommand;