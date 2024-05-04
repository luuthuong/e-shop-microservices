using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Core.Infrastructure.Api;
using Core.Infrastructure.CQRS;
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

builder.Services.AddApiEndpoints(Assembly.GetCallingAssembly());
        
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();
builder.Services.AddCQRS();

var tokenIssuerSettings = builder.Configuration.GetSection("TokenIssuerSettings");
builder.Services.Configure<TokenIssuerSettings>(tokenIssuerSettings);

builder.Services.AddScoped<AppIdentityDbContext>();
builder.Services.AddScoped<ITokenRequester, TokenRequester>();
builder.Services.AddScoped<IIdentityManager, IdentityManager>();
builder.Services.AddTransient<IProfileService, CustomProfileService>();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");
var migrationsAssembly = typeof(Program)
    .GetTypeInfo().Assembly.GetName().Name;

// DbContext
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

// Authorization and Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});

builder.Services.AddIdentityServer(opt =>
        opt.IssuerUri = tokenIssuerSettings.GetValue<string>("Authority"))
    .AddDeveloperSigningCredential() // without a certificate, for dev only
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        options.EnableTokenCleanup = true;
    })
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddAspNetIdentity<User>()
    .AddProfileService<CustomProfileService>();

// Cors
builder.Services.AddCors(o =>
    o.AddPolicy("CorsPolicy", policyBuilder =>
        {
            policyBuilder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200");
        }
    ));

builder.Services.AddApiVersioning(
        options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

// App
var app = builder.Build();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder routeGroupBuilder = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);
app.MapApiEndpoints(routeGroupBuilder);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
}

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseIdentityServer();
app.UseAuthorization();

app.MigrateDatabase().Run();