using Core.CQRS.Command;
using ProductSyncService.Application.Products.Commands;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Application.Products.CommandHandlers;

public class CreateProductHandler(IProductRepository productRepository) : ICommandHandler<CreateProduct>
{
    public async Task Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.CategoryId,
            request.Description,
            request.ShortDescription);
        await productRepository.InsertAsync(product, cancellationToken:  cancellationToken);
    }
}