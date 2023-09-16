using Application.DTO;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Database.Interface;
using MediatR;

namespace Application.CQRS.Command.Categories;

public record AddCategoryCommand(AddCategoryRequest Request) : IRequest<CategoryDTO>;

public sealed class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();
        RuleFor(x => x.Request.Name).NotNull().NotEmpty();
    }
}

internal sealed class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryDTO>
{
    private readonly IAppDbContext _appDbContext;

    public AddCategoryCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<CategoryDTO> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var entry = _appDbContext.Category.Add(Category.Create(request.Request.Name));
        var entity = entry.Entity;
        return Task.FromResult(new CategoryDTO()
        {
            Id = entity.Id,
            Name = entity.Name,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate
        });
    }
}
