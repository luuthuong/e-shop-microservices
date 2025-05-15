using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text;
using Core.Outbox;
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
    private readonly DebeziumSetting _debeziumSettings =
        debeziumOptions.Value ?? throw new ArgumentNullException(nameof(debeziumOptions));
    
    static readonly HttpClient httpClient = new HttpClient();

    public async Task ConfigureAsync(CancellationToken cancellationToken = default)
    {
        var retryPolicy = Policy.Handle<HttpRequestException>()
            .WaitAndRetryForeverAsync(attempt => TimeSpan.FromSeconds(5));
        
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromHours(1));
        
        var policyWrap = Policy.WrapAsync(timeoutPolicy, retryPolicy);

        await policyWrap.ExecuteAsync(async () =>
        {
            var debeziumConfig = new JObject()
            {
                { "connector.class", _debeziumSettings.ConnectorClass },
                { "task.max", 2 },
                // Database config
                { "database.server.name", _debeziumSettings.DatabaseServerName },
                { "database.hostname", _debeziumSettings.DatabaseHostname },
                { "database.port", _debeziumSettings.DatabasePort },
                { "database.user", _debeziumSettings.DatabaseUser },
                { "database.password", _debeziumSettings.DatabasePassword },
                { "database.names", _debeziumSettings.DatabaseName },
                { "database.encrypt", false },
                { "table.include.list", _debeziumSettings.TableIncludeList },
                { "topic.prefix", _debeziumSettings.TopicPrefix },
                { "schema.history.internal.kafka.bootstrap.servers", _debeziumSettings.KafkaServer },
                { "schema.history.internal.kafka.topic", "schema-changes" },
                // Transforms
                { "transforms", "unwrap,route" },
                { "transforms.unwrap.type", "io.debezium.transforms.ExtractNewRecordState" },
                { "transforms.unwrap.drop.tombstones", "true" },
                { "transforms.unwrap.delete.handling.mode", "drop" },
                { "transforms.unwrap.remove.fields", "source,ts_ms,transaction,op" },
                { "transforms.route.type", "org.apache.kafka.connect.transforms.RegexRouter" },
                { "transforms.route.regex", $"{_debeziumSettings.TopicPrefix}.(.*).{_debeziumSettings.TableIncludeList}" },
                { "transforms.route.replacement", _debeziumSettings.TopicReplacement },
                { "key.converter.schemas.enable", false },
                { "value.converter.schemas.enable", false },
                { "value.converter", "org.apache.kafka.connect.json.JsonConverter" },
                { "key.converter", "org.apache.kafka.connect.json.JsonConverter" },
            };

            var debeziumConfigData = JsonConvert.SerializeObject(debeziumConfig);
            
            var configContent = new StringContent(
                JsonConvert.SerializeObject(debeziumConfig),
                null,
                MediaTypeNames.Application.Json);

            logger.LogInformation("Configuring debezium with config: {debeziumConfigData}", debeziumConfigData);

            var request = new HttpRequestMessage(HttpMethod.Put, $"{_debeziumSettings.ConnectorUrl}/config")
            {
                Content = configContent,
            };

            var response = await httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Was not possible to configure Debezium. Status code: {statusCode}", response.StatusCode);
                logger.LogError(await response.Content.ReadAsStringAsync(cancellationToken));
            }
        });
    }

    public static void Demo([StringSyntax(StringSyntaxAttribute.Json)] string json)
    {
    }
}