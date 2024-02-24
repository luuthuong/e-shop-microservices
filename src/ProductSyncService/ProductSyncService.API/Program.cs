using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Infrastructure.AutoMapper;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.EF.DbContext;
using Core.Infrastructure.Quartz;
using Core.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Infrastructure.Configs;
using ProductSyncService.Infrastructure.Outbox;
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
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            config.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
        }
    )
    .AddAutoMapper()
    .AddQuartzJob<OutBoxMessageJob>()
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