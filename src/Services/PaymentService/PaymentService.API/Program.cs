using Core;
using Core.Identity;
using Core.Infrastructure;
using Core.Infrastructure.Api;
using Core.Infrastructure.EF;
using Core.Infrastructure.Kafka;
using Core.Infrastructure.Serilog;
using Infrastructure.Configs;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.ConfigureOptions<AppSettingSetup>();

builder.Services.AddCoreInfrastructure<PaymentDbContext>(builder.Configuration);

builder.Services.AddKafkaConsumer(builder.Configuration);

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

await app.MigrateDbAsync<PaymentDbContext>();

app.Run();