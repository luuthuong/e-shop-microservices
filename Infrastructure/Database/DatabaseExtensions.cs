using System.Reflection;
using Core.Mediator;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Database.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database;

public static class DatabaseExtensions
{
    public static async Task<IServiceCollection> ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(config =>
        {
            string? connectionString = configuration.GetConnectionString("Database");
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
        
        services.AddIdentity<User, Role>().AddEntityFrameworkStores<AppDbContext>();
        
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        
        var autoMigrate = configuration.GetSection("AutoMigrate").Get<bool>();
        if (autoMigrate)
        {
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Migrate Done!");
        }
        services.AddScoped<IAppDbContext, AppDbContext>();
        return services;
    }
    
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        return services; 
    }
}