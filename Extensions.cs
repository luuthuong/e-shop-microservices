using Domain.CQRS.Behavior;
using Domain.Database;
using Domain.Database.Interface;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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