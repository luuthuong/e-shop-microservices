using System.Linq.Expressions;
using AutoMapper;
using Core.Utils;
using EntityGraphQL.Schema;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Domain.Entities;
using ProductSyncService.Domain.EntityErrors;
using ProductSyncService.DTO;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.GraphQL.Mutations;

public sealed class ProductMutations
{
    private readonly IMapper _mapper;

    public ProductMutations(IMapper mapper)
    {
        _mapper = mapper;
    }

    [GraphQLMutation("create new product")]
    public async Task<Expression<Func<IAppDbContext, ProductDTO>>> CreateProduct(IAppDbContext ctx, string name, Guid typeId, string description = "")
    {
        var entity = Product.Create(name, typeId, description);
        ctx.Product.Add(entity);
        await ctx.SaveChangeAsync();
        return  (dbContext) => _mapper.Map<Product, ProductDTO>(dbContext.Product.First(p => p.Id == entity.Id));
    }

    [GraphQLMutation("update product")]
    public async Task<Expression<Func<IAppDbContext,Product>>> UpdateProduct(IAppDbContext dbContext, [GraphQLArguments]UpdateProductArgs request, Guid id)
    {
        var currentProduct = await dbContext.Product.FirstOrDefaultAsync(p => p.Id == id);
        if (currentProduct is null)
            return (ctx) => null;
        currentProduct.Update(request.Name, request.Description, request.Published, 0, 0);
        await dbContext.SaveChangeAsync();
        return (ctx) =>  ctx.Product.First(x => x.Id == currentProduct.Id);
    }
}

public record UpdateProductArgs(string Name, bool Published = true, string Description = "", string ShortDescription= "");
