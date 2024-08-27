using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Core.Outbox;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;

namespace Core.Infrastructure.Outbox;

public class DebeziumConnectorConfiguration(
    IOptions<DebeziumSetting> debeziumOptions,
    ILogger<DebeziumConnectorConfiguration> logger
    ) : IDebeziumConnectorConfiguration
{
    private readonly DebeziumSetting _debeziumSettings = debeziumOptions.Value ?? throw new ArgumentNullException(nameof(debeziumOptions));

    public async Task ConfigureAsync(CancellationToken cancellationToken = default)
    {
        var retryPolicy = Policy.Handle<HttpRequestException>()
            .WaitAndRetryForeverAsync(attempt => TimeSpan.FromSeconds(5));
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromHours(1));
        var policyWrap = Policy.WrapAsync(timeoutPolicy, retryPolicy);

        await policyWrap.ExecuteAsync(async () =>
        {
            using var httpClient = new HttpClient();
            var debeziumConfig = new JObject()
            {
                {"connector.class", _debeziumSettings.ConnectorClass},
                {"task.max", 1},
                {"database.hostname", _debeziumSettings.DatabaseHostname},
                {"database.port", _debeziumSettings.DatabasePort},
                {"database.user", _debeziumSettings.DatabaseUser},
                {"database.password", _debeziumSettings.DatabasePassword},
                {"database.dbname", _debeziumSettings.DatabaseName},
                {"database.server.name", _debeziumSettings.DatabaseServerName},
                {"schema.include.list", _debeziumSettings.SchemaIncludeList},
                {"table.include.list", _debeziumSettings.TableIncludeList},
                {"transforms.outbox.route.topic.replacement", _debeziumSettings.TransformsTopicReplacement },
                {"tombstones.on.delete", "false" },
                {"transforms", "outbox"}
            };
            var _debeziumConfig = "";

            var configContent = new StringContent(
                JsonConvert.SerializeObject(debeziumConfig),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            logger.LogInformation("Configuring debezium with config: {debeziumConfig}", nameof(debeziumConfig));

            var response = await httpClient.PutAsync($"{_debeziumSettings.ConnectorUrl}/config", configContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Was not possible to configure Debezium. Status code: {statusCode}", response.StatusCode);
                throw new System.Exception("Was not possible to configure Debezium outbox.");
            }
        });
    }

    public static void Demo([StringSyntax(StringSyntaxAttribute.Json)]string json){

    }
}
