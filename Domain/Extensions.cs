using System.Text;
using Domain.CQRS.Behavior;
using Domain.Database;
using Domain.Database.Interface;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Domain;

public static class Extensions
{
    public static string GetDbConnection()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
 
        string strConnection = builder.Build().GetConnectionString("Database");
        return strConnection;
    }

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSetting = configuration.GetSection("JWTSetting");
        services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearerDefault(jwtSetting);
        return services;
    }

    private static AuthenticationBuilder AddJwtBearerDefault(this AuthenticationBuilder auth, IConfigurationSection jwtSetting)
    {
        return auth.AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = jwtSetting["validIssuer"],
                ValidAudience = jwtSetting["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtSetting["securityKey"] ?? string.Empty
                    )
                ),
            };
        });
    }

    private static AuthenticationBuilder AddOpenIdConnectDefault(this AuthenticationBuilder auth,
        IConfigurationSection jwtSetting)
    {
        return auth.AddOpenIdConnect(JwtBearerDefaults.AuthenticationScheme, opts =>
        {
        });
    }

    public static async Task<IServiceCollection> ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(config =>
        {
            string connectionString = configuration.GetConnectionString("Database");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            Console.WriteLine($"Connection String: {connectionString}");
            config.UseSqlServer(connectionString, sqlConfig =>
            {
                sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
            });
        });
        
        //Add Identity
        services.AddIdentity<User, Role>().AddEntityFrameworkStores<AppDbContext>();
        
        Console.WriteLine("Migrating ...");
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Migrate Done!");
        services.AddScoped<IAppDbContext, AppDbContext>();
        return services;
    }

    public static IServiceCollection ConfigureMediatr(this IServiceCollection service)
    {
        service.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });
        service.AddValidatorsFromAssembly(typeof(Extensions).Assembly);
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        return service;
    }
}