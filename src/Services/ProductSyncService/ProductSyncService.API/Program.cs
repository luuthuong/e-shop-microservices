using Core;
using Core.Identity;
using Core.Infrastructure;
using Core.Infrastructure.Api;
using Core.Infrastructure.EF;
using Core.Infrastructure.Serilog;
using Core.Redis;
using ProductSyncService.Infrastructure.Configs;
using ProductSyncService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);

builder.Services.ConfigureOptions<AppSettingSetup>();

builder.Services.AddCoreInfrastructure<ProductSyncDbContext>(builder.Configuration);

builder.Services.AddAuthorization(
    (options) =>
    {
        options.AddPolicy(PolicyConstants.M2MAccess, AuthPolicyBuilder.M2MAccess);
        options.AddPolicy(PolicyConstants.CanWrite, AuthPolicyBuilder.CanWrite);
        options.AddPolicy(PolicyConstants.CanRead, AuthPolicyBuilder.CanRead);
    }
);

var app = builder.Build();

app.UseMinimalApi(builder.Configuration);

app.UseAppSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

app.UseSerilogUI();

await app.MigrateDbAsync<ProductSyncDbContext>();

app.MapGet("/test-redis", (ICacheService cacheService) =>
{
    var result = cacheService.TryGetThenSet("key", () => "123");
});

app.Run();