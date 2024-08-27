using System;
using Core.Outbox;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Outbox.Worker;

public class DebeziumServiceWorker(
    ILogger<BackgroundServiceWorker> logger,
    IDebeziumConnectorConfiguration debeziumConfiguration
) : BackgroundServiceWorker(logger, debeziumConfiguration.ConfigureAsync)
{
}
