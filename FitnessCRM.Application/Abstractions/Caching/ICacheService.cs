namespace FitnessCRM.Application.Abstractions.Caching;

public interface ICacheService
{
    Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken);
    Task SetAsync<TValue>(string key, TValue value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken);
}