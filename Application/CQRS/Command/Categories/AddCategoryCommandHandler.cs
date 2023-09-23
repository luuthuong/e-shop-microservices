using Application.DTO;
using Application.Helpers;
using AutoMapper;
using Core.Mediator;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Database.Interface;

namespace Application.CQRS.Command.Categories;

public record AddCategoryCommand(AddCategoryRequest Request) : BaseRequest<AddCategoryResponse>;

public sealed class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();
        RuleFor(x => x.Request.Name).NotNull().NotEmpty();
    }
}

internal sealed class AddCategoryCommandHandler : BaseRequestHandler<AddCategoryCommand, AddCategoryResponse>
{
    public AddCategoryCommandHandler(IMapper mapper, IAppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override Task<AddCategoryResponse> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var entry = DBContext.Category.Add(Category.Create(request.Request.Name));
        var result = Mapper.Map<Category, CategoryDTO>(entry.Entity);
        return Task.FromResult(
            new AddCategoryResponse()
            {
                Data = result
            }
        );
    }
}