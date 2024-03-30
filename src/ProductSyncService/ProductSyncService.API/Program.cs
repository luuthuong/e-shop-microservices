using System.Reflection;
using Core.Infrastructure.Api;
using Core.Infrastructure.AutoMapper;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.EF;
using Core.Infrastructure.Outbox.Worker;
using Core.Infrastructure.Quartz;
using Core.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Infrastructure.Configs;
using ProductSyncService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var appSetting = builder.Configuration.Get<AppSettings>()!;

builder.Logging.AddConsole();
builder.Services
    .ConfigureOptions<AppSettingSetup>()
    .AddAppDbContext<ProductSyncDbContext>(
        config =>
        {
            if (string.IsNullOrEmpty(appSetting.ConnectionStrings.Database))
                throw new ArgumentNullException();

            Console.WriteLine($"Connection String: {appSetting.ConnectionStrings.Database}");
            return config.UseSqlServer(appSetting.ConnectionStrings.Database, sqlConfig =>
            {
                sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
            });
        }
    )
    .AddRedis(appSetting.Redis)
    .AddRepositories(appSetting.Redis.Enable)
    .AddCQRS(
        config =>
        {
            config.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
        }
    )
    .AddAutoMapper()
    .AddQuartzJob<OutBoxMessageJob<ProductSyncDbContext>>()
    .AddEndpointsApiExplorer()
    .AddHttpContextAccessor()
    .AddSwaggerGen()
    .AddAuthorization()
    .AddAuthentication();

await builder.Services.MigrateDbAsync<ProductSyncDbContext>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.AddApiEndpoints(Assembly.GetExecutingAssembly());

app.Run();