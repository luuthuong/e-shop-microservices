using AutoMapper;
using FluentValidation;
using ProductSyncService.Application.DTO;
using ProductSyncService.Application.Helpers;
using ProductSyncService.Domain.Entities;
using ProductSyncService.DTO;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.CQRS.Products.Commands;

public sealed record AddProductCommand(AddProductRequest Request) : BaseRequest<AddProductResponse>;
public sealed class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(x => x.Request.Name).MinimumLength(5);
    }
}

internal sealed class AddProductCommandHandler:  BaseRequestHandler<AddProductCommand, AddProductResponse>
{

    private readonly IProductRepository _productRepository;
    public AddProductCommandHandler(IMapper mapper, IAppDbContext dbContext, IProductRepository productRepository) : base(mapper, dbContext)
    {
        _productRepository = productRepository;
    }

    public override async Task<AddProductResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var rq = request.Request;
        var entity = Product.Create(
            rq.Name,
            rq.CategoryId,
            rq.Description, "",
            published: true
        );
        var result = await _productRepository.InsertAsync(entity, cancellationToken: cancellationToken);
        return new AddProductResponse()
        {
            Success = true,
            Data = Mapper.Map<Product, ProductDTO>(result)
        };
    }
}