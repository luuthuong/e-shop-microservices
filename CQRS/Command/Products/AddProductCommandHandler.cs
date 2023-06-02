using Domain.Database.Interface;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.CQRS.Command.Products;

public class AddProductCommand: IRequest<ProductDTO>
{
    public AddProductRequest Request { get; set; }
}

internal sealed class AddProductCommandHandler:  IRequestHandler<AddProductCommand, ProductDTO>
{
    private readonly IAppDbContext _appDbContext;

    public AddProductCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ProductDTO> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var rq = request.Request;

        var result = _appDbContext.Product.Add(Product.Create(rq.Name, rq.Count, Price.Create("vnd", 0), rq.CategoryId)).Entity;
        await _appDbContext.SaveChangeAsync(cancellationToken);
        return new ProductDTO()
        {
            Name = result.Name,
            Count = result.Count,
            Price = result.Price,
            Id = result.Id,
            CreatedDate = result.CreatedDate,
            UpdatedDate = result.UpdatedDate
        };
    }
}