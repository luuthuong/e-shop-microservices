using Core.Identity;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Identity.API.Requests;
using Identity.Domains;
using Identity.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

        var tokenSettings = scope.ServiceProvider.GetRequiredService<IOptions<IdentityTokenIssuerSettings>>();

        if (!configurationDbContext.Clients.Any())
        {
            foreach (var client in IdentityConfiguration.GetClients(tokenSettings.Value))
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

        // seeding user
        IIdentityManager identityManager = scope.ServiceProvider.GetRequiredService<IIdentityManager>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        RegisterUserRequest adminUser = new()
        {
            Email = "administrator@gmail.com",
            Password = "G3n$&t7W!qX9vM4d",
            PasswordConfirm = "G3n$&t7W!qX9vM4d"
        };

        var adminAccount = await userManager.FindByEmailAsync(adminUser.Email);

        if (adminAccount is null)
        {
            var result = await identityManager.RegisterUserAdmin(adminUser);
            if (!result.Succeeded)
                throw new ApplicationException(result.Errors.First().Description);
        }
    }
}

