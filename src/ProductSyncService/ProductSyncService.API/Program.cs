using System.Text.Json;
using System.Text.Json.Serialization;
using API;
using ProductSyncService.Infrastructure.Configs;
using ProductSyncService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<AppSettingSetup>();

builder.Logging.AddConsole();
await builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AddMediatR();
builder.Services.AddAutoMapper();
builder.Services.AddRedis();
builder.Services.AddQuartz();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    option.JsonSerializerOptions.IncludeFields = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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