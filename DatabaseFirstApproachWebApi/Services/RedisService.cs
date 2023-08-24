using Microsoft.Extensions.Caching.Distributed;

namespace DatabaseFirstApproachWebApi.Services
{
	public class RedisService : IRedisService
	{
		IDistributedCache _cache;
		public RedisService(IDistributedCache cache)
		{
			_cache = cache;
		}
		public async Task<string> GetAsync(string key)
		{
			var value = await _cache.GetStringAsync(key);
			if (!string.IsNullOrEmpty(value))
			{
				return value;
			}
			return default;
		}
		public async Task AddAsync(string key, string value)
		{
			await _cache.SetStringAsync(key, value);
		}
		public async Task DeleteAsync(string key)
		{
			await _cache.RemoveAsync(key);
		}


	}
}
