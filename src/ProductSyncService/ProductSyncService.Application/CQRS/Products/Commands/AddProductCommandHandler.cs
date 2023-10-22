using AutoMapper;
using FluentValidation;
using ProductSyncService.Application.DTO;
using ProductSyncService.Application.Helpers;
using ProductSyncService.Domain.Entities;
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
    public AddProductCommandHandler(IMapper mapper, IAppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override async Task<AddProductResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var rq = request.Request;
        var result = DBContext.Product.Add(
            Product.Create(
                rq.Name, 
                rq.CategoryId,
                rq.Description,"",
                published: true
                )).Entity;
        await DBContext.SaveChangeAsync(cancellationToken);
        return new AddProductResponse()
        {
            Success = true,
            Data = Mapper.Map<Product, ProductDTO>(result)
        };
    }
}