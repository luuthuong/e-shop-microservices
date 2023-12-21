using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Infrastructure.Utils;

public class PrivateResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty prop = base.CreateProperty(member, memberSerialization);

        if (!prop.Writable)
        {
            var property = member as PropertyInfo;
            bool hasPrivateSetter = property?.GetGetMethod(true) != null;
            prop.Writable = hasPrivateSetter;
        }
        return prop;
    }
}


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