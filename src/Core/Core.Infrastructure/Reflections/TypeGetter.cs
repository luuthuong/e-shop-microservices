using Core.Infrastructure.Utils;

namespace Core.Infrastructure.Reflections;

public static class TypeGetter
{
    public static Type? GetTypeFromCurrentDomainAssembly(string typeName)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(AssemblyUtils.GetLoadableTypes)
            .FirstOrDefault(t => !t.IsAbstract && t.Name == typeName);
    }
}