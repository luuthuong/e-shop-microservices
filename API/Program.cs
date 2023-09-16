using API;
using Application;
using Core;
using Core.AutoMapper;
using Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AutoMapperConfigure();
await builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.RegisterMediatR();
builder.Services.RegisterAutoMapper();
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