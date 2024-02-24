using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Core.Infrastructure;

public class WebApiBuilder
{
    public static WebApplication Build(string[] args, Action<WebApplicationBuilder> callback)
    {
        var builder = WebApplication.CreateBuilder(args);
        callback(builder);
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        return app;
    }
}