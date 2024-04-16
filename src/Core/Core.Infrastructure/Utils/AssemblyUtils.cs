using System.Collections;
using System.Reflection;

namespace Core.Infrastructure.Utils;

public static class AssemblyUtils
{
    public static IEnumerable<Assembly> GetUsedAssemblies(this Type target, bool ignoreOriginAssembly = false)
    {
        bool IsIgnoreAssembly(Assembly assembly) => !ignoreOriginAssembly || assembly != target.Assembly;

        bool FromType(Type type) => type.GetInterfaces().Contains(target)
                                    || type.IsSubclassOf(target)
                                    || target.IsSubclassOf(type)
                                    || target.IsAssignableFrom(type);

        bool FromGenericType(Type type, Type genericType) =>
            (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            || type.GetInterfaces().Any(
                itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == genericType
            )
            || (
                type.BaseType is { IsGenericType: true }
                && type.BaseType.GetGenericTypeDefinition() == genericType
            );

        return Assembly.GetEntryAssembly()!.GetReferencedAssemblies().Select(Assembly.Load)
            .Where(IsIgnoreAssembly)
            .Where(
                assembly => assembly.GetTypes().Any(
                    type => (target.IsGenericType && FromGenericType(type, target))
                            || FromType(type)
                )
            );
    }

    public static Assembly[] GetAssembliesFromTypes(bool ignoreOriginAssembly = false,  params Type[] types)
    {
        List<Assembly> assemblies = new List<Assembly>();
        foreach (var type in types)
        {
            var result = GetUsedAssemblies(type, ignoreOriginAssembly).ToList();
            if (result.Any())
                assemblies.AddRange(result);
        }

        return assemblies.ToArray();
    }

    public static IEnumerable<Type> GetTypesFromBaseType(params Type[] types)
    {
        List<Type> childTypeResults = new List<Type>();

        foreach (var type in types)
        {
            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
            {
                if (Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(file);
                        var childTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(type)).ToList();
                        if (!childTypes.Any())
                            continue;
                        childTypeResults.AddRange(childTypes);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Failed to load assembly: {file}. {ex.Message}");
                    }
                }
            }
        }

        return childTypeResults;
    }

    public static IEnumerable<Assembly> LoadAssemblyFromLocation(string path)
    {
        foreach (var file in Directory.GetFiles(path, "*.dll"))
        {
            yield return Assembly.LoadFile(file);
        }
    }

    public static Assembly?[] GetAssembliesOfType(params Type[] type)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Where(
                x => type.Any(
                    y => y.IsAssignableFrom(x)) && x is { IsClass: true, IsAbstract: false }
            )
            .Select(Assembly.GetAssembly!)
            .Distinct()
            .ToArray();
    }


    public static Type[] GetTypeAssignableFrom(params Type[] type)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Where(x => type.Any(y => y.IsAssignableFrom(x)) && x is
            {
                IsClass: true, 
                IsAbstract: false
            })
            .Select(x => x)
            .ToArray();
    }

    public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null).ToList()!;
        }
    }
}