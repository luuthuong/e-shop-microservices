using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.BaseDbContext;

public static class DbContextExtension
{
    public static IServiceCollection ConfigureDbContext<TContext>(
        this IServiceCollection services, 
        IConfiguration configuration, 
        Func<DbContextOptionsBuilder, DbContextOptionsBuilder>? action = null
        ) where TContext: BaseDbContext
    {
        services.AddSingleton<DomainEventsToOutboxMessageInterceptor>();
        
        services.AddDbContext<TContext>(config =>
        {
            var interceptor = services.BuildServiceProvider().GetService<DomainEventsToOutboxMessageInterceptor>();
            if (action is not null)
                action(config);
            
            if (interceptor != null) 
                config.AddInterceptors(interceptor);
        });
        
        var items = Assembly.GetCallingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(BaseDbContext)))
            .Select(type =>
            {
                var itf = type.GetInterfaces()
                    .FirstOrDefault(x => x.GetInterfaces().Contains(typeof(IBaseDbContext)));
                return (type, itf);
            });
        
        foreach (var (context, itf) in items)
        {
            if (itf is not null)
                services.AddScoped(itf, context);
        }
        
        return services;
    }
}