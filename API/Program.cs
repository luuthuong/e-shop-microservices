using System.Reflection;
using API;
using Core;
using Core.AutoMapper;
using Core.Mediator;
using Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AutoMapperConfigure();

await builder.Services.ConfigureDbContext(builder.Configuration);

builder.Services.ConfigureMediatR(Assembly.GetEntryAssembly());

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddScoped<JwtHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();