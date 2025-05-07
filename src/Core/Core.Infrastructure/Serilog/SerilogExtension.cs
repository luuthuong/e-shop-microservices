using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Ui.MsSqlServerProvider;
using Serilog.Ui.Web;

namespace Core.Infrastructure.Serilog;

public static class SerilogExtension
{
    public static void EnableSerilog(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog(
            (s, lc) => lc
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(s)
                .Enrich.FromLogContext()
        );

        var connectionString = builder.Configuration.GetConnectionString("Database");
        Log.Information("Serilog connection to database: {connectionString}", connectionString);

        if (!string.IsNullOrEmpty(connectionString))
            builder.Services.AddSerilogUi(
                (options) =>
                {
                    Log.Information($"Serilog connection to database: {connectionString}");
                    options.UseSqlServer(connectionString, "Serilog");
                }
            );
        
    }

    public static void UseSerilogUI(this WebApplication app)
    {
        app.UseSerilogRequestLogging()
            .UseSerilogUi(
                options =>
                {
                    options.RoutePrefix = "serilog-ui";
                    options.HomeUrl = "/";
                    options.Authorization = new AuthorizationOptions()
                    {
                        AuthenticationType = AuthenticationType.Jwt,
                        RunAuthorizationFilterOnAppRoutes = true,
                        Filters =
                        [
                        ]
                    };
                }
            );
    }
}