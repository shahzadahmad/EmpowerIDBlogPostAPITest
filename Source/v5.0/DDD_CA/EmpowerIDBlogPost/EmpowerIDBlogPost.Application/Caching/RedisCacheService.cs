using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;

        public RedisCacheService(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var db = _redisConnection.GetDatabase();
            var cachedValue = await db.StringGetAsync(key);

            if (cachedValue.HasValue)
            {
                return JsonSerializer.Deserialize<T>(cachedValue.ToString());
            }

            return default; // Return default value if not found in cache
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var db = _redisConnection.GetDatabase();
            var serializedValue = JsonSerializer.Serialize(value);

            if (absoluteExpiration.HasValue)
            {
                await db.StringSetAsync(key, serializedValue, absoluteExpiration);
            }
            else
            {
                await db.StringSetAsync(key, serializedValue);
            }
        }

        public async Task RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var db = _redisConnection.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
