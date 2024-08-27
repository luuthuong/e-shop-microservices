using Core.CQRS.Command;
using Core.Results;
using ProductSyncService.Domain.Categories;

namespace ProductSyncService.Application.Products;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    string ShortDescription,
    CategoryId CategoryId
) : ICommand;