using System;
using System.Threading;
using System.Threading.Tasks;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Infrastructure.Realisations.Caching.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FitnessCRM.Infrastructure.Realisations.Caching;

public class CacheService(IMemoryCache cache, IOptions<CacheOptions> options) : ICacheService
{
    private readonly IMemoryCache _cache = cache;
    private readonly CacheOptions _options = options.Value;

    public Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken)
    {
        _cache.TryGetValue(key, out TValue? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<TValue>(string key, TValue value, TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        _cache.Set(key, value, TimeSpan.FromSeconds(expiration?.TotalSeconds ?? _options.DefaultExpirationSeconds));
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }
}