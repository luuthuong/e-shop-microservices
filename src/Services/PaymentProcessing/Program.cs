using Core;
using Core.Identity;
using Core.Infrastructure;
using Core.Infrastructure.Api;
using Core.Infrastructure.EF;
using Core.Infrastructure.Kafka;
using Core.Infrastructure.Outbox;
using Core.Infrastructure.Serilog;
using Microsoft.EntityFrameworkCore;
using PaymentProcessing.Domain;
using PaymentProcessing.Infrastructure;
using PaymentProcessing.Infrastructure.Projections;
using Serilog;

var builder = WebApplication.CreateSlimBuilder(args);

var appSettings = builder.Configuration.LoadAppSettings();

builder.EnableSerilog();

builder.Services.AddCoreInfrastructure(builder.Configuration);


builder.Services.AddEventSourcing<Payment>(appSettings.ConnectionStrings.Database);

builder.Services.AddQueryRepository<Payment, PaymentReadDbContext>();

builder.Services.AddDBContext<PaymentReadDbContext>(
    config =>
    {
        var database = appSettings.ConnectionStrings.Database;

        if (string.IsNullOrEmpty(database))
            throw new ArgumentNullException();

        Log.Information($"Connection String: {database}");

        return config.UseSqlServer(
            database,
            sqlConfig => sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null)
        );
    }
);

builder.Services.AddDebeziumWorker(builder.Configuration);
builder.Services.AddKafkaConsumer(builder.Configuration);
builder.Services.AddProjection<PaymentDetailsProjection>();

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

await app.MigrateDbAsync<PaymentReadDbContext>();

app.Run();