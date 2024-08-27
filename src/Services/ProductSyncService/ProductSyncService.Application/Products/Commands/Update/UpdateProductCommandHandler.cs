using Core.CQRS.Command;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Application.Products;

internal sealed class UpdateProductCommandHandler(IProductRepository productRepository): ICommandHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            throw new NotImplementedException();
        }
        
        await productRepository.UpdateAsync(product, cancellationToken);
    }
}