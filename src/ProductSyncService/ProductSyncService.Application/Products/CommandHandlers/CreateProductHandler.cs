using Core.CQRS.Command;
using ProductSyncService.Application.Products.Commands;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Application.Products.CommandHandlers;

public class CreateProductHandler:  ICommandHandler<CreateProduct>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.CategoryId,
            request.Description,
            request.ShortDescription);
        await _productRepository.InsertAsync(product, cancellationToken:  cancellationToken);
    }
}