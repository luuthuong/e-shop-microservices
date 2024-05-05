using Core.Infrastructure;
using Core.Infrastructure.Api;
using Core.Infrastructure.EF;
using CustomerService.Infrastructure.Configs;
using CustomerService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var appSetting = builder.Configuration.Get<AppSettings>()!;
builder.Logging.AddConsole();

builder.Services.ConfigureOptions<AppSettingSetup>();

builder.Services.AddCoreInfrastructure<CustomerDbContext>(appSetting);

var app = builder.Build();

var routeGroupBuilder = app.MapGroupWithApiVersioning(1);

app.MapApiEndpoints(routeGroupBuilder);

app.UseSwagger(onlyDevelopment: true);

await app.MigrateDbAsync<CustomerDbContext>();
app.Run();