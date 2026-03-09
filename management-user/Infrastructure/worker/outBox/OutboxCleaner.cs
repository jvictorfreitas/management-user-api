using Infrastructure.database;
using Microsoft.EntityFrameworkCore;

namespace worker.outbox;

public class OutboxCleaner : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxCleaner> _logger;

    public OutboxCleaner(IServiceScopeFactory scopeFactory, ILogger<OutboxCleaner> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Clean(stoppingToken);

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task Clean(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var retention = DateTime.UtcNow.AddHours(-24);

        var messages = await context
            .OutboxMessages.Where(x => x.ProcessedAt != null && x.ProcessedAt < retention)
            .Take(500)
            .ToListAsync(stoppingToken);

        if (messages.Count == 0)
            return;

        context.OutboxMessages.RemoveRange(messages);

        await context.SaveChangesAsync(stoppingToken);

        _logger.LogInformation("OutboxCleaner removed {Count} messages", messages.Count);
    }
}
