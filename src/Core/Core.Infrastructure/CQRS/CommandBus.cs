using Core.CQRS.Command;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.CQRS;
public class CommandBus(IMediator mediator, ILogger<CommandBus> logger) : ICommandBus
{
    public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        logger.LogDebug("Executing Command: {command}", command);
        return mediator.Send(command);
    }
}