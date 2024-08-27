using Newtonsoft.Json.Linq;

namespace Core.Outbox;

public class DebeziumSetting
{
    public required string ConnectorUrl { get; set; }
    public required string DatabaseHostname { get; set; }
    public required string DatabaseServerName { get; set; }
    public required string DatabasePort { get; set; }
    public required string DatabaseUser { get; set; }
    public required string DatabasePassword { get; set; }
    public required string DatabaseName { get; set; }
    public required string TopicPrefix { get; set; }
    public required string TransformsTopicReplacement { get; set; }
    public required string SlotName { get; set; }
    public required string SchemaIncludeList { get; set; }
    public required string TableIncludeList { get; set; }
    public required string ConnectorClass { get; set; }
}
