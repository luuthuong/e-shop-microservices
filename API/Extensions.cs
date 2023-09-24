using System.Reflection;
using System.Text;
using Core.Mediator;
using FluentValidation;
using Infrastructure.BackgroundJobs;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace API;
public static class Extensions
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSetting = configuration.GetSection("JWTSetting");
        services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearerDefault(jwtSetting);
        return services;
    }

    public static IServiceCollection ConfigureQuartz(this IServiceCollection services)
    {
         services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(OutBoxMessageJob));

            config.AddJob<OutBoxMessageJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(5).RepeatForever()
                    ));
            config.UseMicrosoftDependencyInjectionJobFactory();
        });
         services.AddQuartzHostedService();
         return services;
    }

    private static AuthenticationBuilder AddJwtBearerDefault(this AuthenticationBuilder auth, IConfigurationSection jwtSetting)
    {
        return auth.AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = jwtSetting["validIssuer"],
                ValidAudience = jwtSetting["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtSetting["securityKey"] ?? string.Empty
                    )
                ),
            };
        });
    }
    

    private static AuthenticationBuilder AddOpenIdConnectDefault(this AuthenticationBuilder auth,
        IConfigurationSection jwtSetting)
    {
        return auth.AddOpenIdConnect(JwtBearerDefaults.AuthenticationScheme, opts =>
        {
        });
    }
}