namespace Core.CQRS.Command;

public interface ICommandBus
{
    Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
}