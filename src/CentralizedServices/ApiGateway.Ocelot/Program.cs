using Core.Infrastructure.Serilog;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.EnableSerilog();

builder.Services.AddHealthChecks();

builder.Configuration.AddJsonFile("ocelot.json", reloadOnChange: true, optional: false);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddCacheManager(
        options =>
        {
            options.WithDictionaryHandle();
        }
    );

var app = builder.Build();

app.UseHttpsRedirection();

app.UseOcelot().Wait();

// app.UseSerilogUI();

app.Run();