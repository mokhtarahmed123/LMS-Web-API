namespace LMS.Infrastructure.Caching.Redis
{
    public interface ICacheService
    {
        Task<T?> GetDataAsync<T>(string key);
        Task SetDataAsync<T>(string key, T value, TimeSpan? time = null);
        Task RemoveAsync(string key);
    }
}
