using System.Reflection;
using Core.EF;
using Core.Infrastructure.EF.DbContext;
using Core.Infrastructure.EF.Repository;
using Core.Infrastructure.Outbox.Interceptor;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.EF;

public static class EFExtension
{
    public static IServiceCollection AddAppDbContext<TContext>(
        this IServiceCollection services,
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
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<TContext>));
        return services;
    }

    public static async Task MigrateDbAsync<TDbContext>(this WebApplication app)
        where TDbContext : BaseDbContext
    {
        using var scope = app.Services.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        try
        {
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Migrate Done!");
        }
        catch (System.Exception e)
        {
            // ignored
        }

        var seeders = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(AssemblyUtils.GetLoadableTypes)
            .Where(
                type => type.GetInterfaces()
                            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDbSeeder<>))
                        && type is
                        {
                            IsClass: true,
                            IsAbstract: false
                        }
            )
            .Select(x => (IDbSeeder<TDbContext>)Activator.CreateInstance(x)!)
            .OrderBy(x => x.Key)
            .ToList();

        for (int i = 0; i < seeders.Count; i++)
        {
            var seeder = seeders[i];
            var existed = await dbContext.SeedingHistory.AnyAsync(x => x.Key.Equals(seeder.Key));
            if (existed)
                continue;
            await seeder.DoAsync(dbContext, scope.ServiceProvider);
            await dbContext.SeedingHistory.AddAsync(new()
            {
                Key = seeder.Key,
                EntityName = seeder.GetType().Name
            });
            await dbContext.SaveChangesAsync();
        }
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, bool useCache)
    {
        var repositories = AssemblyUtils.GetTypeAssignableFrom(typeof(BaseRootRepository));
        if (repositories.Length == 0)
            return services;
        foreach (var repository in repositories)
        {
            var itf = repository.GetInterfaces().LastOrDefault();
            if (itf is null)
                continue;

            // if (repository.BaseType != null && !useCache &&
            //     repository.BaseType.Name.Contains(typeof(RepositoryCache<,>).Name))
            //     continue;

            services.AddScoped(itf, repository);

            // if (!useCache || !repository.Name.Contains("Cache"))
            //     continue;
            // services.Decorate(itf, repository);
        }

        return services;
    }

    // public static IServiceCollection AddUnitOfWork(this IServiceCollection service)
    // {
    //     // var unitOfWork = AppDomain.CurrentDomain
    //     //     .GetAssemblies()
    //     //     .SelectMany(AssemblyUtils.GetLoadableTypes)
    //     //     .Where(t => t is
    //     //         {
    //     //             IsClass: true,
    //     //             IsAbstract: false
    //     //         } && t.GetInterfaces().Any(itf =>
    //     //             itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IUnitOfWork<>)
    //     //         )
    //     //     );
    //
    //     // var unitOfWork = Assembly.GetCallingAssembly().GetReferencedAssemblies()
    //     //     .Select(Assembly.Load)
    //     //     .SelectMany(AssemblyUtils.GetLoadableTypes)
    //     //     .Where(t => t is
    //     //         {
    //     //             IsClass: true,
    //     //             IsAbstract: false
    //     //         } && t.GetInterfaces().Any(itf =>
    //     //             itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IUnitOfWork<>)
    //     //         )
    //     //     );
    //     //
    //     // return service;
    // }

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