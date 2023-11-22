using Core.CQRS.Command;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.CQRS;
public class CommandBus: ICommandBus
{
    private readonly IMediator _mediator;
    private readonly ILogger<CommandBus> _logger;

    public CommandBus(IMediator mediator , ILogger<CommandBus> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        _logger.LogInformation("Command: {command} sending...", command);
        return _mediator.Send(command);
    }
}