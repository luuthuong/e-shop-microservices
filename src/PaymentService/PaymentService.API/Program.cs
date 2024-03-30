using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Infrastructure.AutoMapper;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.EF;
using Core.Infrastructure.EF.DbContext;
using Core.Infrastructure.Quartz;
using Infrastructure.BackgroundJob;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAppDbContext<AppDbContext>(
        config =>
        {
            string? connectionString = builder.Configuration.GetConnectionString("Database");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            Console.WriteLine($"Connection String: {connectionString}");
            return config.UseSqlServer(connectionString, sqlConfig =>
            {
                sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
            });
        }
    ).AddRepositories(false)
    .AddCQRS()
    .AddAutoMapper()
    .AddQuartzJob<OutboxMessageJob>()
    .AddEndpointsApiExplorer()
    .AddHttpContextAccessor()
    .AddSwaggerGen()
    .AddControllers()
    .AddJsonOptions(option =>
    {
        option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        option.JsonSerializerOptions.IncludeFields = true;
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();