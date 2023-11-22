using API;
using Application;
using Core.Infrastructure.Quartz;
using Infrastructure.BackgroundJob;
using Infrastructure.Database;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

await builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.RegisterMediatR();
builder.Services.RegisterAutoMapper();
builder.Services.ConfigureQuartz(config =>
{
    var jobKey = new JobKey(nameof(OutboxMessageJob));
    config.AddJob<OutboxMessageJob>(jobKey)
        .AddTrigger(trigger => trigger.ForJob(jobKey)
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(5).RepeatForever()));
});

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