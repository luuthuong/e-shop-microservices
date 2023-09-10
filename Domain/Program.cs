using Domain.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Domain;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        await builder.Services.ConfigureDbContext(builder.Configuration);
        builder.Services.ConfigureMediatr();
        builder.Services.ConfigureAuthentication(builder.Configuration);
        builder.Services.AddScoped<JwtHandler>();
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        await app.RunAsync();
    }
}