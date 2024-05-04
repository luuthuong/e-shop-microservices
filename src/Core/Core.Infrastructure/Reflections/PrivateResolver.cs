using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Infrastructure.Reflections;
// https://medium.com/@mahruj66/deserializing-public-property-with-private-setter-in-net-8ffef1effc62
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
