using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Outbox.Worker;

public delegate Task BackgroundServiceWorkerAction(CancellationToken cancellationToken);

public abstract class BackgroundServiceWorker(
    ILogger<BackgroundServiceWorker> logger,
    BackgroundServiceWorkerAction action): BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken cancellationToken) =>
        Task.Run(
            async () =>
            {
                await Task.Yield();
                logger.LogInformation("Background worker started...");
                await action(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("Background worker stopped");
            },
            cancellationToken
        );
}
