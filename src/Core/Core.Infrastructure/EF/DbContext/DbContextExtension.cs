using System.Reflection;
using Core.EF;
using Core.Infrastructure.EF.Repository;
using Core.Infrastructure.Outbox.Interceptor;
using Core.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.EF.DbContext;

public static class DbContextExtension
{
    public static async Task<IServiceCollection> ConfigureDbContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<DbContextOptionsBuilder, DbContextOptionsBuilder>? action = null
    ) where TContext : BaseDbContext
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

        // Repository register
        var repositories = AssemblyUtils
            .GetTypeAssignableFrom(typeof(BaseRootRepository));
        if (repositories.Any())
            foreach (var repository in repositories)
            {
                var itf = repository.GetInterfaces().LastOrDefault();
                if (itf is null)
                    continue;
                if (repository.Name.Contains("Cache"))
                {
                    services.Decorate(itf, repository);
                    continue;
                }

                services.AddScoped(itf, repository);
            }

        var dbContext = services.BuildServiceProvider().GetRequiredService<TContext>();

        var autoMigrate = configuration.GetSection("AutoMigrate").Get<bool>();
        if (autoMigrate)
        {
            try
            {
                await dbContext.Database.MigrateAsync();
                Console.WriteLine("Migrate Done!");
            }
            catch (System.Exception e)
            {
                // ignored
            }
        }

        var seeders = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(
                type => type.GetInterfaces().Any(
                            i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDbSeeder<>)
                        )
                        && type is { IsClass: true, IsAbstract: false }
            ).Select(x => (IDbSeeder<TContext>)Activator.CreateInstance(x)!).ToList();

        for (int i = 0; i < seeders.Count; i++)
        {
            var seeder = seeders[i];
            var existed = await dbContext.SeedingHistory.AnyAsync(x => x.Key.Equals(seeder.Key));
            if (existed)
                continue;
            await seeder.DoAsync(dbContext, services.BuildServiceProvider());
            await dbContext.SeedingHistory.AddAsync(new()
            {
                Key = seeder.Key,
                EntityName = seeder.GetType().Name
            });
            await dbContext.SaveChangesAsync();
        }

        return services;
    }

    public static void AddRepositories(this IServiceCollection services, bool useCache)
    {
        var repositories = AssemblyUtils
            .GetTypeAssignableFrom(typeof(BaseRootRepository));
        if (repositories.Any())
            foreach (var repository in repositories)
            {
                var itf = repository.GetInterfaces().LastOrDefault();
                if (itf is null)
                    continue;
                services.AddScoped(itf, repository);
            }
    }

    public static void MigrationScript(this MigrationBuilder migrationBuilder)
    {
        Assembly assembly = Assembly.GetCallingAssembly();
        string[] files = assembly.GetManifestResourceNames();
        if (!files.Any())
            return;
        string prefix = $"{assembly.GetName().Name}.Migrations.Scripts.";
        var scriptFiles = files.Where(f => f.StartsWith(prefix) && f.EndsWith(".sql"))
            .Select(file => new { ScriptFile = file, FileName = file.Replace(prefix, String.Empty) })
            .OrderBy(file => file.FileName);
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