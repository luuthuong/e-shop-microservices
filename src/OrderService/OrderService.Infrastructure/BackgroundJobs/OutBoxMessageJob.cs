using Core.Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class OutBoxMessageJob: IJob
{
    private readonly AppDbContext _appDbContext;
    private readonly IPublisher _publisher;

    public OutBoxMessageJob(AppDbContext appDbContext, IPublisher publisher)
    {
        _appDbContext = appDbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _appDbContext.OutboxMessage
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
            await _publisher.Publish(domainEvent, context.CancellationToken);
            message.ProcessedOnUtc = DateTime.Now;
        }
        await _appDbContext.SaveChangeAsync();
    }
}