using System.Text.Json;
using System.Text.Json.Serialization;
using API;
using Core.Infrastructure.Quartz;
using CustomerService.Infrastructure.Outbox;
using CustomerService.Infrastructure.Persistence;
using Quartz;

var builder = WebApplication.CreateBuilder(args);


await builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.RegisterMediatR();
builder.Services.RegisterAutoMapper();

builder.Services.ConfigureQuartz(
    config =>
    {
        var jobKey = new JobKey(nameof(OutBoxMessageJob));
        config.AddJob<OutBoxMessageJob>(jobKey)
            .AddTrigger(
                trigger => trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(5)
                            .RepeatForever()
                    )
            );
    }
);

builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    option.JsonSerializerOptions.IncludeFields = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();