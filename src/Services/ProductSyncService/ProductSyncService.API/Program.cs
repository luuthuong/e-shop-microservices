using Core;
using Core.Identity;
using Core.Infrastructure;
using Core.Infrastructure.Api;
using Core.Infrastructure.EF;
using Core.Infrastructure.Serilog;
using ProductSyncService.Infrastructure.Configs;
using ProductSyncService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var appSetting = builder.Configuration.Get<AppSettings>()!;

builder.Services.ConfigureOptions<AppSettingSetup>();

builder.Services.AddCoreInfrastructure<ProductSyncDbContext>(appSetting);

builder.Services.ConfigureSerilog(builder.Configuration);

builder.Services.AddAuthorization(
    (options) =>
    {
        options.AddPolicy(PolicyConstants.M2MAccess, AuthPolicyBuilder.M2MAccess);
        options.AddPolicy(PolicyConstants.CanWrite, AuthPolicyBuilder.CanWrite);
        options.AddPolicy(PolicyConstants.CanRead, AuthPolicyBuilder.CanRead);
    }
);

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapApiEndpoints(app.MapGroupWithApiVersioning(1));

app.UseSwagger(onlyDevelopment: true);

app.UseSerilogUI();

await app.MigrateDbAsync<ProductSyncDbContext>();

app.Run();