using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.CQRS;

public sealed class LoggingBehavior<TRequest>: IRequestPreProcessor<TRequest> where TRequest: notnull
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest>> logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {name} {@request}", typeof(TRequest).Name, request);
        return Task.CompletedTask;
    }
}