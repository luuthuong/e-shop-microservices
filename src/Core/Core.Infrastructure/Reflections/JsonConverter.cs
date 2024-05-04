using Newtonsoft.Json;

namespace Core.Infrastructure.Reflections;

public sealed class JsonConverter
{
    public static string Stringify(object value)
    {
        return JsonConvert.SerializeObject(value);
    }

    public static T? Parse<T>(string? json)
    {
        if (json != null)
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
            {
                ContractResolver = new PrivateResolver()
            });
        return default;
    }
}