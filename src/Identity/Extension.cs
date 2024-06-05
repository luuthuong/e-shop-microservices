using System.Reflection;
using Identity.Domains;
using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Identity;

public static class Extension
{
    public static IServiceCollection AddIdentityServer(this IServiceCollection services, string connectionString
        , string authority)
    {
        var assembly = Assembly.GetExecutingAssembly().GetName().Name!;
        services.AddIdentityServer(
                options => options.IssuerUri = authority
            )
            .AddDeveloperSigningCredential() // without a certificate, for dev only
            .AddOperationalStore(
                options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(assembly)
                        );
                    options.EnableTokenCleanup = true;
                })
            .AddConfigurationStore(
                options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(
                        connectionString,
                        sql => sql.MigrationsAssembly(assembly)
                    );
                })
            .AddAspNetIdentity<User>()
            .AddProfileService<CustomProfileService>();

        return services;
    }
}