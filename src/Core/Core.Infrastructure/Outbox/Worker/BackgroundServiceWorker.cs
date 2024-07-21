using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Outbox.Worker;

public abstract class BackgroundServiceWorker(
    ILogger<BackgroundServiceWorker> logger,
    Func<CancellationToken, Task> action): BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Run(
            async () =>
            {
                await Task.Yield();
                logger.LogInformation("Background worker started...");
                await action(stoppingToken).ConfigureAwait(false);
                logger.LogInformation("Background worker stopped");
            },
            stoppingToken
        );
}