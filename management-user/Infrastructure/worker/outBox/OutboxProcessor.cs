using Infrastructure.database;
using Microsoft.EntityFrameworkCore;
using queue.rabbit;

namespace worker.outbox;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OutboxProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var messages = await context
                .OutboxMessages.Where(x => x.ProcessedAt == null)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                await publisher.PublishAsync(message.Type, message.Payload);

                message.ProcessedAt = DateTime.UtcNow;
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }
}
