using System.Reflection;
using Core.BaseRepository;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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
        
        // db context register
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
        
        // Repository register
        var repositories = AssemblyUtils
            .GetTypeAssignableFrom(typeof(BaseRootRepository));
        if(repositories.Any())
            foreach (var repository in repositories)
            {
                var itf = repository.GetInterfaces().LastOrDefault();
                if (itf is not null)
                    services.AddScoped(itf, repository);
            }
        return services;
    }
    
    public static void MigrationScript(this MigrationBuilder migrationBuilder)
    {
        Assembly assembly = Assembly.GetCallingAssembly();
        string[] files = assembly.GetManifestResourceNames();
        if (!files.Any())
            return;
        string prefix = $"{assembly.GetName().Name}.Migrations.Scripts.";
        var scriptFiles = files.Where(f => f.StartsWith(prefix) && f.EndsWith(".sql")).Select(file => new { ScriptFile = file, FileName = file.Replace(prefix, String.Empty) }).OrderBy(file => file.FileName);
        foreach (var file in scriptFiles)
        {
            using var stream = assembly.GetManifestResourceStream(file.ScriptFile);
            using var reader = new StreamReader(stream!);
            var command = reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(command))
                continue;
            migrationBuilder.Sql(command);
        }
    }
}