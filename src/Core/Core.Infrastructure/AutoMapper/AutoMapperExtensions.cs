using System.Reflection;
using AutoMapper;
using Core.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.AutoMapper;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var assemblies = typeof(Profile).GetUsedAssemblies(true).ToList();
        return services.AddAutoMapper(assemblies);
    }

    public static IEnumerable<TDestination> MapEnumerable<TEntity, TDestination>(this IMapper mapper, IEnumerable<TEntity> entities) where TEntity : notnull 
        => mapper.Map<IEnumerable<TEntity>, IEnumerable<TDestination>>(entities);
}