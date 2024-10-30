using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using TecnoMundo.Application.DTOs;

namespace TecnoMundo.ProductAPI.Caching
{
    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _distributedCache;

        public CachingService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<List<T>> GetListCache<T>(string keyCache)
        {
            var itemList = await _distributedCache.GetAsync(keyCache);

            if (itemList is not null)
            {
                var serialize = Encoding.UTF8.GetString(itemList);
                return JsonConvert.DeserializeObject<List<T>>(serialize) ?? [];
            }

            return [];
        }

        public async Task<T?> GetItemInCache<T>(string keyCache)
        {
            var item = await _distributedCache.GetAsync(keyCache);

            if (item is not null)
            {
                var serialize = Encoding.UTF8.GetString(item);
                return JsonConvert.DeserializeObject<T>(serialize);
            }

            return default(T);
        }

        public async Task<List<T>> AddListInCache<T>(List<T> item, string keyCache, DistributedCacheEntryOptions options)
        {
            var serializeItem = JsonConvert.SerializeObject(item);
            var redisProducts = Encoding.UTF8.GetBytes(serializeItem);

            await _distributedCache.SetAsync(keyCache, redisProducts, options);

            return await GetListCache<T>(keyCache);
        }

        public async Task AddItemToExistingListInCache<T>(T item, string keyCache, DistributedCacheEntryOptions options)
        {
            var itemListInCache = await GetListCache<T>(keyCache);

            if (itemListInCache.Count == 0) return;

            itemListInCache.Add(item);
            await AddListInCache(itemListInCache, keyCache, options);
        }

        public async Task<T?> AddItemInCache<T>(T item, string keyCache, DistributedCacheEntryOptions options)
        {
            var serializeItem = JsonConvert.SerializeObject(item);
            var redisProducts = Encoding.UTF8.GetBytes(serializeItem);

            await _distributedCache.SetAsync(keyCache, redisProducts, options);

            return await GetItemInCache<T>(keyCache);
        }

        public async Task RemoveExistingListItemFromCache<T>(T item, List<T> listOfItemToBeRemoved, string keyCache, DistributedCacheEntryOptions options)
        {
            listOfItemToBeRemoved.RemoveAll(x => EqualityComparer<T>.Default.Equals(x, item) ||
                (typeof(T).GetProperty("Id")?.GetValue(x)?.Equals(typeof(T).GetProperty("Id")?.GetValue(item)) ?? false));
            await AddListInCache(listOfItemToBeRemoved, keyCache, options);
        }

        public async Task UpdateExistingListItemFromCache<T>(T item, List<T> listOfItemToBeUpdated, string keyCache, DistributedCacheEntryOptions options)
        {
            int indexItemToBeUpdated = listOfItemToBeUpdated.FindIndex(x => EqualityComparer<T>.Default.Equals(x, item) ||
                (typeof(T).GetProperty("Id")?.GetValue(x)?.Equals(typeof(T).GetProperty("Id")?.GetValue(item)) ?? false));

            if (indexItemToBeUpdated != -1)
            {
                listOfItemToBeUpdated[indexItemToBeUpdated] = item;
                await AddListInCache(listOfItemToBeUpdated, keyCache, options);
            }
        }

        public async Task RemoveItemInCache(string keyCache)
        {
            await _distributedCache.RemoveAsync(keyCache);
        }

        public async Task UpdateItemInCache<T>(T item, string keyCache, DistributedCacheEntryOptions options)
        {
            await AddItemInCache(item, keyCache, options);
        }
    }
}
