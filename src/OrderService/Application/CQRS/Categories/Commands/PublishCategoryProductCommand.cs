using Application.DTO;
using Application.Helpers;
using AutoMapper;
using Infrastructure.Database.Interface;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Commands;

public sealed record PublishCategoryProductCommand(Guid CategoryId, Guid ProductId) : BaseRequest<PublishCategoryProductResponse>;

internal sealed class PublishCategoryProductCommandHandler: BaseRequestHandler<PublishCategoryProductCommand, PublishCategoryProductResponse>
{
    public PublishCategoryProductCommandHandler(IMapper mapper, IAppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override async Task<PublishCategoryProductResponse> Handle(PublishCategoryProductCommand request, CancellationToken cancellationToken)
    {
        var category =await DBContext.Category.Include(c => c.ListProducts).FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null)
            return new()
            {
                Success = false,
                ErrorMsg = "Category Not Found."
            };
        
        var product = category.ListProducts.FirstOrDefault(p => p.Id == request.ProductId);
        if(product is null)
            return new()
            {
                Success = false,
                ErrorMsg = "Product Not Found."
            };
        category.PublishProduct(product);
        return new()
        {
            Success = true,
        };
    }
}