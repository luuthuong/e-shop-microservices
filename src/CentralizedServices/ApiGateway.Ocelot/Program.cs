using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

builder.Configuration.AddJsonFile("ocelot.json");

builder.Services
    .AddOcelot(builder.Configuration)
    .AddCacheManager(
        options =>
        {
            options.WithDictionaryHandle();
        }
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseOcelot().Wait();

app.Run();