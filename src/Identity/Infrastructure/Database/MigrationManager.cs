using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Identity.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public static class MigrationManager
{
    //https://code-maze.com/migrate-identityserver4-configuration-to-database/
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        
        using var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        persistedGrantDbContext.Database.Migrate();

        using var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        configurationDbContext.Database.Migrate();

        using var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        identityDbContext.Database.Migrate();

        if (!configurationDbContext.Clients.Any())
        {
            foreach (var client in IdentityConfiguration.Clients)
                configurationDbContext.Clients.Add(client.ToEntity());

            configurationDbContext.SaveChanges();
        }

        if (!configurationDbContext.IdentityResources.Any())
        {
            foreach (var resource in IdentityConfiguration.IdentityResources)
                configurationDbContext.IdentityResources.Add(resource.ToEntity());

            configurationDbContext.SaveChanges();
        }

        if (!configurationDbContext.ApiResources.Any())
        {
            foreach (var resource in IdentityConfiguration.ApiResources)
                configurationDbContext.ApiResources.Add(resource.ToEntity());

            configurationDbContext.SaveChanges();
        }

        if (!configurationDbContext.ApiScopes.Any())
        {
            foreach (var apiScope in IdentityConfiguration.ApiScopes)
                configurationDbContext.ApiScopes.Add(apiScope.ToEntity());

            configurationDbContext.SaveChanges();
        }

        return host;
    }
}

