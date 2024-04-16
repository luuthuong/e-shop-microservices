using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
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

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder routeGroupBuilder = app.MapGroup("/api/v{version:apiVersion}").WithApiVersionSet(apiVersionSet);

app.MapApiEndpoints(routeGroupBuilder);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
}
await app.MigrateDbAsync<CustomerDbContext>();
app.Run();