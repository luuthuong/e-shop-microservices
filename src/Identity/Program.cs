using System.Reflection;
using Core.Infrastructure.Api;
using Core.Infrastructure.Identity;
using Duende.IdentityServer.Services;
using Identity.Domains;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var service = builder.Services;

service.AddEndpointsApiExplorer();

service.AddApiEndpoints(Assembly.GetEntryAssembly()!);

service.AddMemoryCache();

var tokenIssuerSettings = builder.Configuration.GetSection("TokenIssuerSettings");
service.Configure<TokenIssuerSettings>(tokenIssuerSettings);

service.AddScoped<AppIdentityDbContext>();
service.AddScoped<ITokenRequester, TokenRequester>();
service.AddScoped<IIdentityManager, IdentityManager>();
service.AddTransient<IProfileService, CustomProfileService>();

service.AddSwaggerGen(
    option => option.EnableAnnotations()
    );
;

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");
var migrationsAssembly = typeof(Program)
    .GetTypeInfo().Assembly.GetName().Name;

service.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

service.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

service.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});

service.AddIdentityServer(
        options => options.IssuerUri = tokenIssuerSettings.GetValue<string>("Authority")
    )
    .AddDeveloperSigningCredential() // without a certificate, for dev only
    .AddOperationalStore(
        options =>
        {
            options.ConfigureDbContext = b =>
                b.UseSqlServer(
                    connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly)
                );
            options.EnableTokenCleanup = true;
        })
    .AddConfigurationStore(
        options =>
        {
            options.ConfigureDbContext = b => b.UseSqlServer(
                connectionString,
                sql => sql.MigrationsAssembly(migrationsAssembly)
            );
        })
    .AddAspNetIdentity<User>()
    .AddProfileService<CustomProfileService>();

service.AddCors(
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

service.AddVersioningApi();

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