using System.Collections.Concurrent;

namespace WeatherAPIIntegration.Domain.Services
{
    public class CacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, (object value, DateTime expiration)> _cache = new();

        public Task<T> GetAsync<T>(string key)
        {
            Console.WriteLine($"Attempting to retrieve key: {key}"); // Debugging line

            if (_cache.TryGetValue(key, out var cacheEntry))
            {
                Console.WriteLine($"Cache hit for key: {key}"); // Debugging line
                if (cacheEntry.expiration > DateTime.UtcNow)
                {
                    Console.WriteLine($"Cache entry is valid for key: {key}"); // Debugging line
                    return Task.FromResult((T)cacheEntry.value);
                }
                else
                {
                    Console.WriteLine($"Cache entry expired for key: {key}"); // Debugging line
                }
            }
            else
            {
                Console.WriteLine($"Cache miss for key: {key}"); // Debugging line
            }

            return Task.FromResult(default(T));
        }

        public Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            Console.WriteLine($"Setting cache for key: {key} with expiration: {expiration}"); // Debugging line
            var cacheEntry = (value, DateTime.UtcNow.Add(expiration));
            _cache[key] = cacheEntry;
            return Task.CompletedTask;
        }
    }
}
