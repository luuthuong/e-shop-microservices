using Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductSyncService.Infrastructure.Persistence;
using Quartz;

namespace ProductSyncService.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public sealed class OutBoxMessageJob: IJob
{
    private readonly ProductSyncDbContext _dbContext;
    private readonly IPublisher _publisher;

    public OutBoxMessageJob(ProductSyncDbContext appDbContext, IPublisher publisher)
    {
        _dbContext = appDbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.OutboxMessage
            .Where(m => m.ProcessedOnUtc == null)
            .Take(10)
            .ToListAsync(context.CancellationToken);
        foreach (var message in messages)
        {
             IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            if(domainEvent is null)
                continue;
            await _publisher.Publish(domainEvent, context.CancellationToken);
            message.ProcessedOnUtc = DateTime.Now;
        }
        await _dbContext.SaveChangeAsync();
    }
}