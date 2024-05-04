using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Identity.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public static class MigrationManager
{
    //https://code-maze.com/migrate-identityserver4-configuration-to-database/
    public static async Task MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        await using var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        await persistedGrantDbContext.Database.MigrateAsync();

        await using var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        await configurationDbContext.Database.MigrateAsync();

        await using var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        await identityDbContext.Database.MigrateAsync();
        
        if (!configurationDbContext.Clients.Any())
        {
            foreach (var client in IdentityConfiguration.Clients)
                configurationDbContext.Clients.Add(client.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }

        if (!configurationDbContext.IdentityResources.Any())
        {
            foreach (var resource in IdentityConfiguration.IdentityResources)
                configurationDbContext.IdentityResources.Add(resource.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }

        if (!configurationDbContext.ApiResources.Any())
        {
            foreach (var resource in IdentityConfiguration.ApiResources)
                configurationDbContext.ApiResources.Add(resource.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }

        if (!configurationDbContext.ApiScopes.Any())
        {
            foreach (var apiScope in IdentityConfiguration.ApiScopes)
                configurationDbContext.ApiScopes.Add(apiScope.ToEntity());

            await configurationDbContext.SaveChangesAsync();
        }
    }
}

