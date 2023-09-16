using System.Reflection;

namespace Core.Utils;

public static class AssemblyUtils
{
    public static Assembly[] GetAssembliesFromBaseType(params Type[] types)
    {
        List<Assembly> assemblies = new List<Assembly>();

        foreach (var type in types)
        {
            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
            {
                if (Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(file);
                        var hasChildTypes = assembly.GetTypes().Any(x => x.IsSubclassOf(type));

                        if (!hasChildTypes) continue;
                        assemblies.Add(assembly);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load assembly: {file}. {ex.Message}");
                    }
                }
            }
        }
        Console.WriteLine(assemblies.Count);

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
                        var childTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(type));

                        if (childTypes == null || !childTypes.Any()) continue;

                        childTypeResults.AddRange(childTypes);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load assembly: {file}. {ex.Message}");
                    }
                }
            }
        }

        return childTypeResults;
    }

    public static Assembly[] GetAssembliesOfType(params Type[] type)
    {
        var result = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Where(x => type.Any(y => y.IsAssignableFrom(x)) && x.IsClass && !x.IsAbstract)
            .Select(Assembly.GetAssembly)
            .Distinct()
            .ToArray();

        return result;
    }


    public static Type[] GetTypeAssignableFrom(params Type[] type)
    {
        var result = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => type.Any(y => y.IsAssignableFrom(x)) && x.IsClass && !x.IsAbstract)
            .Select(x => x)
            .ToArray();

        return result;
    }

    private static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null);
        }
    }
}