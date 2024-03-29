using API;
using Application;
using Core.Quartz;
using Infrastructure.BackgroundJobs;
using Infrastructure.Database;
using Quartz;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

await builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.RegisterMediatR();
builder.Services.RegisterAutoMapper();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddScoped<JwtHandler>();
builder.Services.ConfigureQuartz(config =>
{
    var jobKey = new JobKey(nameof(OutBoxMessageJob));
    config.AddJob<OutBoxMessageJob>(jobKey)
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