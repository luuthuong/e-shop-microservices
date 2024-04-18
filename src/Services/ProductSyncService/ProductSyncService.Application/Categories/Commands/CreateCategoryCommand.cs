using Core.CQRS.Command;

namespace ProductSyncService.Application.Categories.Commands;

public record CreateCategoryCommand(string Name) : ICommand;