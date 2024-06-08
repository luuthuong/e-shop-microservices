using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Ui.MsSqlServerProvider;
using Serilog.Ui.Web;

namespace Core.Infrastructure.Serilog;

public static class SerilogExtension
{
    public static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog(
            (s, lc) => lc
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(s)
                .Enrich.FromLogContext()
        );

        var connectionString = configuration.GetConnectionString("Database");
        if (!string.IsNullOrEmpty(connectionString))
            services.AddSerilogUi(
                (options) => { options.UseSqlServer(connectionString, "Serilog"); }
            );
        return services;
    }

    public static void UseSerilogUI(this WebApplication app)
    {
        app.UseSerilogRequestLogging()
            .UseSerilogUi(
                options =>
                {
                    options.RoutePrefix = "serilog-ui";
                    options.HomeUrl = "/swagger/index.html";
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