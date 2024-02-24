using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.CQRS;

public sealed class LoggingBehavior<TRequest>(ILogger<LoggingBehavior<TRequest>> logger)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request: {name} {@request}", typeof(TRequest).Name, request);
        return Task.CompletedTask;
    }
}