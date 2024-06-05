using Core;
using Core.Identity;
using Core.Infrastructure;
using Core.Infrastructure.Api;
using Core.Infrastructure.EF;
using CustomerService.Infrastructure.Configs;
using CustomerService.Infrastructure.Persistence;
using Duende.IdentityServer.Services;

var builder = WebApplication.CreateBuilder(args);
var appSetting = builder.Configuration.Get<AppSettings>()!;
builder.Logging.AddConsole();

builder.Services.ConfigureOptions<AppSettingSetup>();

builder.Services.AddCoreInfrastructure<CustomerDbContext>(appSetting);
builder.Services.AddHealthChecks();

builder.Services.AddAuthorization(
    (options) => {
        options.AddPolicy(PolicyConstants.M2MAccess, AuthPolicyBuilder.M2MAccess);
        options.AddPolicy(PolicyConstants.CanWrite, AuthPolicyBuilder.CanWrite);
        options.AddPolicy(PolicyConstants.CanRead, AuthPolicyBuilder.CanRead);
    }
);

var app = builder.Build();


app.UseSwagger(onlyDevelopment: true);

await app.MigrateDbAsync<CustomerDbContext>();

app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health-checks");

var routeGroupBuilder = app.MapGroupWithApiVersioning(1);
app.MapApiEndpoints(routeGroupBuilder);

app.Run();