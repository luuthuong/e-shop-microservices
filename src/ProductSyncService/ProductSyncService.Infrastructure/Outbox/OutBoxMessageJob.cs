using Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductSyncService.Infrastructure.Persistence;
using Quartz;

namespace ProductSyncService.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public sealed class OutBoxMessageJob(
    ProductSyncDbContext appDbContext, 
    IPublisher publisher) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await appDbContext.OutboxMessage
            .Where(m => m.ProcessedOnUtc == null)
            .Take(10)
            .OrderByDescending(x => x.ExecutedOnUtc)
            .ToListAsync(context.CancellationToken);
        foreach (var message in messages)
        {
             IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content, new JsonSerializerSettings
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