namespace Core.Infrastructure.Utils;

public static class EnumerableExtension
{
    public static TDest? FirstOrDefaultOfType<TDest>(this IEnumerable<dynamic> source) where TDest : notnull
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.OfType<TDest>().FirstOrDefault();
    }
}