using System.Text;
using Confluent.Kafka;
using Core.Infrastructure.Reflections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Infrastructure.Kafka.Serialization;

public class JsonEventSerializer<T> : ISerializer<T>, IDeserializer<T> where T : class
{
    public byte[] Serialize(T data, SerializationContext context) =>
        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            return null!;

        var jsonString = Encoding.UTF8.GetString(data.ToArray());
        var jsonObject = JObject.Parse(jsonString);

        var eventName = jsonObject["EventName"]?.ToString();

        if (string.IsNullOrEmpty(eventName))
            throw new JsonSerializationException("EventName is missing in the JSON payload.");

        var eventType = GetEventType(eventName);
        if (eventType == null)
        {
            return null!;
        }

        var eventData = jsonObject["JsonPayload"]?.ToString();

        if (string.IsNullOrWhiteSpace(eventData) || eventData == "{}")
        {
            return null!;
        }

        var @event = JsonConvert.DeserializeObject(eventData!, eventType) as T;
        return @event!;
    }

    private Type? GetEventType(string eventTypeName)
    {
        return TypeGetter.GetTypeFromCurrentDomainAssembly(eventTypeName);
    }
}