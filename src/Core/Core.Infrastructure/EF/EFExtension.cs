using System.Reflection;
using Core.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core.Infrastructure.EF;

public static class EFExtension
{
    public static IServiceCollection AddDBContext<TContext>(
        this IServiceCollection services,
        Func<DbContextOptionsBuilder, DbContextOptionsBuilder>? action = null
    ) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            action?.Invoke(options);
        });
        
        return services;
    }

    public static async Task MigrateDbAsync<TDbContext>(this WebApplication app)
        where TDbContext : DbContext
    {
        Log.Information("Migrating database...");
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await dbContext.Database.MigrateAsync();
        Log.Information("Migrated database!");
    }

    public static void MigrationScript(this MigrationBuilder migrationBuilder)
    {
        var assembly = Assembly.GetCallingAssembly();

        var files = assembly.GetManifestResourceNames();

        if (files.Length == 0)
            return;

        var prefix = $"{assembly.GetName().Name}.Migrations.Scripts.";

        var scriptFiles = files.Where(f => f.StartsWith(prefix) && f.EndsWith(".sql"))
            .Select(file => new { ScriptFile = file, FileName = file.Replace(prefix, string.Empty) })
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

    public static IServiceCollection AddProjection<TProjection>(this IServiceCollection services)
        where TProjection : class, IProjection
    {
        services.AddScoped<TProjection>();
        services.AddScoped<IProjection>(sp => sp.GetRequiredService<TProjection>());
        return services;
    }
}