using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LMS.Infrastructure.Caching.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }
        public async Task<T?> GetDataAsync<T>(string key)
        {
            if (distributedCache is null)
            {
                throw new InvalidOperationException("Distributed cache is not initialized.");
            }

            var data = await distributedCache.GetStringAsync(key);
            if (data is null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        }

        public Task RemoveAsync(string key)
        {
            if (distributedCache is null)
            {
                throw new InvalidOperationException("Distributed cache is not initialized.");
            }
            return distributedCache.RemoveAsync(key);

        }

        public async Task SetDataAsync<T>(string key, T value, TimeSpan? time = null)
        {
            if (distributedCache is null)
            {
                throw new InvalidOperationException("Distributed cache is not initialized.");
            }

            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = time ?? TimeSpan.FromMinutes(10)
            };
            await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
        }
    }
}
