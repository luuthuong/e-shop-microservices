using MediatR;

namespace Core.CQRS.Command;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand: ICommand
{ }