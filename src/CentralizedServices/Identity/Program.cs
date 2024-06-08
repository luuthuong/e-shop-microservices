using System.Reflection;
using Core.Infrastructure.Api;
using Core.Infrastructure.Identity;
using Duende.IdentityServer.Services;
using Identity;
using Identity.Domains;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddEndpointsApiExplorer();

services.AddApiEndpoints(Assembly.GetEntryAssembly()!);

services.AddMemoryCache();

var tokenIssuerSettings = builder.Configuration.GetSection("TokenIssuerSettings");

services.Configure<IdentityTokenIssuerSettings>(tokenIssuerSettings);

services.AddScoped<AppIdentityDbContext>();

services.AddScoped<Core.Identity.ITokenService, TokenService>();

services.AddScoped<IIdentityManager, IdentityManager>();

services.AddTransient<IProfileService, CustomProfileService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"ConnectionString: {connectionString}");

services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

services.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});

services.AddIdentityServer(
    connectionString!,
    tokenIssuerSettings.GetValue<string>("Authority")!
);

services.AddCors(
    options => options.AddPolicy(
        "CorsPolicy",
        policyBuilder =>
        {
            policyBuilder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200");
        }
    )
);

services.AddSwagger(builder.Configuration);

services.AddVersioningApi();

var app = builder.Build();

app.UseMinimalApi(builder.Configuration);

app.UseAppSwaggerUI();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

await app.MigrateDatabase();

app.Run();