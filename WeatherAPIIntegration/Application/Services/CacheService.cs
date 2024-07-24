using System.Collections.Concurrent;

namespace WeatherAPIIntegration.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, (object value, DateTime expiration)> _cache = new();

        public Task<T> GetAsync<T>(string key)
        {

            if (_cache.TryGetValue(key, out var cacheEntry))
            {
                if (cacheEntry.expiration > DateTime.UtcNow)
                {
                    return Task.FromResult((T)cacheEntry.value);
                }
                else
                {
                    Console.WriteLine($"Cache entry expired for key: {key}");
                }
            }
            else
            {
                Console.WriteLine($"Cache miss for key: {key}");
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
