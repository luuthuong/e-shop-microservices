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

services.AddSwaggerGen(
    option => option.EnableAnnotations()
);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"ConnectionString: {connectionString}");

var migrationsAssembly = typeof(Program)
    .GetTypeInfo().Assembly.GetName().Name;

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

services.AddVersioningApi();

var app = builder.Build();

var routerBuilder = app.MapGroupWithApiVersioning(1);

app.MapApiEndpoints(routerBuilder);

app.UseSwagger(onlyDevelopment: true);

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseIdentityServer();
app.UseAuthorization();
await app.MigrateDatabase();

app.Run();