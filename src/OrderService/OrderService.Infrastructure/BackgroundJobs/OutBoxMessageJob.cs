using Core.Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class OutBoxMessageJob(AppDbContext appDbContext, IPublisher publisher) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await appDbContext.OutboxMessage
            .Where(m => m.ProcessedOnUtc == null)
            .Take(10)
            .ToListAsync(context.CancellationToken);
        foreach (var message in messages)
        {
             IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            if(domainEvent is null)
                continue;
            await publisher.Publish(domainEvent, context.CancellationToken);
            message.ProcessedOnUtc = DateTime.Now;
        }
        await appDbContext.SaveChangeAsync();
    }
}