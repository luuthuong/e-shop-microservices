using Core.CQRS.Command;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Application.Products;

public sealed record DeleteProductCommand(ProductId ProductId): ICommand;