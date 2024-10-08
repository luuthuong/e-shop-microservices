﻿using Core.Domain;
using Core.EF;
using Core.Infrastructure.Reflections;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Core.Infrastructure.Outbox.Worker;

public sealed class OutBoxMessageJob<TDBContext>(
    TDBContext appDbContext,
    IPublisher publisher) : IJob where TDBContext : IDbContext
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await appDbContext.OutboxMessage
            .Where(m => m.ProcessedOnUtc == null)
            .OrderByDescending(x => x.ExecutedOnUtc)
            .Take(10)
            .ToListAsync(context.CancellationToken);

        if (!messages.Any())
            return;

        foreach (var message in messages)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new PrivateResolver(),
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                });
            if (domainEvent is null)
                continue;
            await publisher.Publish(domainEvent, context.CancellationToken);
            message.ProcessedOnUtc = DateTime.Now;
        }

        await appDbContext.SaveChangeAsync();
    }
}