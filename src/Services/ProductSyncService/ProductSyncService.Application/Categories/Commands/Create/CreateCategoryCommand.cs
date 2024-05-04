using Core.CQRS.Command;

namespace ProductSyncService.Application.Categories;

public record CreateCategoryCommand(string Name) : ICommand;