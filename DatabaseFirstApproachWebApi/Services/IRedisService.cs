namespace DatabaseFirstApproachWebApi.Services
{
	public interface IRedisService
	{	
		Task<string> GetAsync(string key);
		Task AddAsync(string key, string value);
		Task DeleteAsync(string key);
	}
}
