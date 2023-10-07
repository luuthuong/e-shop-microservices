using Core.BaseDbContext;
using Infrastructure.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database;

public static class DbContextExtension
{
    public static async Task<IServiceCollection> ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext<AppDbContext>(
            configuration,
            config =>
            {
                string? connectionString = configuration.GetConnectionString("Database");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException(nameof(connectionString));
                }
                Console.WriteLine($"Connection String: {connectionString}");
                return config.UseSqlServer(connectionString, sqlConfig =>
                {
                    sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
                });
            });
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<IAppDbContext>();
        var autoMigrate = configuration.GetSection("AutoMigrate").Get<bool>();
        if (autoMigrate)
        {
            try
            {
                await dbContext.Database.MigrateAsync();
                Console.WriteLine("Migrate Done!");
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        return services;
    }
}