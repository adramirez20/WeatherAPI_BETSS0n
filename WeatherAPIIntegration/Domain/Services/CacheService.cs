using System.Collections.Concurrent;

namespace WeatherAPIIntegration.Domain.Services
{
    public class CacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, (object value, DateTime expiration)> _cache = new();

        public Task<T> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out var cacheEntry) && cacheEntry.expiration > DateTime.UtcNow)
            {
                return Task.FromResult((T)cacheEntry.value);
            }

            return Task.FromResult(default(T));
        }

        public Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var cacheEntry = (value, DateTime.UtcNow.Add(expiration));
            _cache[key] = cacheEntry;
            return Task.CompletedTask;
        }
    }
}