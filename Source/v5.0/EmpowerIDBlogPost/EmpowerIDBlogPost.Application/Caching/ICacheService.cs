using System;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.Caching
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data, TimeSpan? cacheDuration = null);
        Task RemoveAsync(string key);
    }
}
