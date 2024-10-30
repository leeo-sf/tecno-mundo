using Microsoft.Extensions.Caching.Distributed;

namespace TecnoMundo.ProductAPI.Caching
{
    public interface ICachingService
    {
        Task<List<T>> AddListInCache<T>(List<T> item, string keyCache, DistributedCacheEntryOptions options);
        Task<List<T>> GetListCache<T>(string keyCache);
        Task<T?> AddItemInCache<T>(T item, string keyCache, DistributedCacheEntryOptions options);
        Task RemoveItemInCache(string keyCache);
        Task UpdateItemInCache<T>(T item, string keyCache, DistributedCacheEntryOptions options);
        Task AddItemToExistingListInCache<T>(T item, string keyCache, DistributedCacheEntryOptions options);
        Task RemoveExistingListItemFromCache<T>(T item, List<T> listOfItemToBeRemoved, string keyCache, DistributedCacheEntryOptions options);
        Task UpdateExistingListItemFromCache<T>(T item, List<T> listOfItemToBeUpdated, string keyCache, DistributedCacheEntryOptions options);
        Task<T?> GetItemInCache<T>(string keyCache);
    }
}
