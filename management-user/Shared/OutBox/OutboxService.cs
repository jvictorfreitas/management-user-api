namespace Shared;

using System.Text.Json;
using Infrastructure.database;

public class OutboxService
{
    private readonly AppDbContext _context;

    public OutboxService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddMessageAsync(string type, object payload)
    {
        var message = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            Payload = JsonSerializer.Serialize(payload),
            CreatedAt = DateTime.UtcNow,
        };

        await _context.OutboxMessages.AddAsync(message);

        await _context.SaveChangesAsync();
    }
}
