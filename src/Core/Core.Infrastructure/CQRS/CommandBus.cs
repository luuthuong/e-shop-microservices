using Core.CQRS.Command;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.CQRS;
public class CommandBus(IMediator mediator, ILogger<CommandBus> logger) : ICommandBus
{
    public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        logger.LogInformation("Command: {command} sending...", command);
        return mediator.Send(command);
    }
}