using System.Reflection;
using Core.Infrastructure.Api;
using Core.Infrastructure.AutoMapper;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.EF;
using Core.Infrastructure.Quartz;
using CustomerService.Infrastructure.Outbox;
using CustomerService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddAppDbContext<CustomerDbContext>(
        config =>
        {
            string? connectionString = builder.Configuration.GetConnectionString("Database");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            Console.WriteLine($"Connection String: {connectionString}");
            return config.UseSqlServer(connectionString,
                sqlConfig => { sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null); });
        }
    )
    .AddCQRS()
    .AddAutoMapper()
    .AddQuartzJob<OutBoxMessageJob>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAuthorization()
    .AddAuthentication();

await builder.Services.MigrateDbAsync<CustomerDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.AddApiEndpoints(Assembly.GetCallingAssembly());

app.Run();