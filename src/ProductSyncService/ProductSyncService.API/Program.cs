using EntityGraphQL.AspNet;
using ProductSyncService.Application;
using ProductSyncService.Infrastructure.Database;
using ProductSyncService.Infrastructure.Database.Interfaces;

var builder = WebApplication.CreateBuilder(args);


await builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.RegisterMediatR();
builder.Services.RegisterAutoMapper();
// builder.Services.ConfigureQuartz(config =>
// {
//     var jobKey = new JobKey(nameof(OutBoxMessageJob));
//     config.AddJob<OutBoxMessageJob>(jobKey)
//         .AddTrigger(trigger => trigger.ForJob(jobKey)
//             .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(5).RepeatForever()));
// });


builder.Services.AddGraphQLSchema<IAppDbContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGraphQL<IAppDbContext>();

app.UseGraphQLPlayground(
    "/graphql",                               // url to host Playground at
    new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/graphql",         // url of GraphQL endpoint
        SubscriptionsEndPoint = "/graphql",   // url of GraphQL endpoint
    });

app.UseAuthorization();

app.MapControllers();

app.Run();