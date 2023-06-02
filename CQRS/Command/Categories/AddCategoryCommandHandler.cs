using Domain.Database.Interface;
using Domain.DTO;
using Domain.Entities;
using MediatR;

namespace Domain.CQRS.Command.Categories;


public record AddCategoryCommand(AddCategoryRequest Request) : IRequest<CategoryDTO>;

public sealed class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryDTO>
{
    private readonly IAppDbContext _appDbContext;

    public AddCategoryCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<CategoryDTO> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var entry = _appDbContext.Category.Add(Category.Create(request.Request.Name));
        await _appDbContext.SaveChangeAsync(cancellationToken);
        var entity = entry.Entity;
        return new CategoryDTO()
        {
            Id = entity.Id,
            Name = entity.Name,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate
        };
    }
}
