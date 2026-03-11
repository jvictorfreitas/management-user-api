using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Shared;

namespace Infrastructure.cache;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var cached = await _cache.GetStringAsync(key);

            if (cached == null)
                return default;

            return JsonSerializer.Deserialize<T?>(cached);
        }
        catch (Exception ex)
        {
            _logger.LogError("REDIS-ERROR: " + ex.Message);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
                options.SetAbsoluteExpiration(expiration.Value);

            var json = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(key, json, options);
        }
        catch (Exception ex)
        {
            _logger.LogError("REDIS-ERROR: " + ex.Message);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogError("REDIS-ERROR: " + ex.Message);
        }
    }
}
