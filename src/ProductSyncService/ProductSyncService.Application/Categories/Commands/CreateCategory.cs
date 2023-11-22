using Core.CQRS.Command;

namespace ProductSyncService.Application.Categories.Commands;

public class CreateCategory: ICommand
{
    private CreateCategory(string name, CancellationToken cancellationToken)
    {
        Name = name;
        CancellationToken = cancellationToken;
    }

    public string Name { get; set; }
    public CancellationToken CancellationToken { get; set; }

    public static CreateCategory Create(
        string name, 
        CancellationToken cancellationToken = default) 
        => new(name, cancellationToken);
}