using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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

        public async Task<List<T>> AddListInCache<T>(
            List<T> item,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var serializeItem = JsonConvert.SerializeObject(item);
            var redisProducts = Encoding.UTF8.GetBytes(serializeItem);

            await _distributedCache.SetAsync(keyCache, redisProducts, options);

            return await GetListCache<T>(keyCache);
        }

        public async Task AddItemToExistingListInCache<T>(
            T item,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var itemListInCache = await GetListCache<T>(keyCache);

            if (itemListInCache.Count == 0)
                return;

            itemListInCache.Add(item);
            await AddListInCache(itemListInCache, keyCache, options);
        }

        public async Task<T?> AddItemInCache<T>(
            T item,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var serializeItem = JsonConvert.SerializeObject(item);
            var redisProducts = Encoding.UTF8.GetBytes(serializeItem);

            await _distributedCache.SetAsync(keyCache, redisProducts, options);

            return await GetItemInCache<T>(keyCache);
        }

        public async Task RemoveExistingListItemFromCache<T>(
            T item,
            List<T> listOfItemToBeRemoved,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            listOfItemToBeRemoved.RemoveAll(x =>
                EqualityComparer<T>.Default.Equals(x, item)
                || (
                    typeof(T)
                        .GetProperty("Id")
                        ?.GetValue(x)
                        ?.Equals(typeof(T).GetProperty("Id")?.GetValue(item)) ?? false
                )
            );
            await AddListInCache(listOfItemToBeRemoved, keyCache, options);
        }

        public async Task UpdateExistingListItemFromCache<T>(
            T item,
            List<T> listOfItemToBeUpdated,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            int indexItemToBeUpdated = listOfItemToBeUpdated.FindIndex(x =>
                EqualityComparer<T>.Default.Equals(x, item)
                || (
                    typeof(T)
                        .GetProperty("Id")
                        ?.GetValue(x)
                        ?.Equals(typeof(T).GetProperty("Id")?.GetValue(item)) ?? false
                )
            );

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

        public async Task UpdateItemInCache<T>(
            T item,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            await AddItemInCache(item, keyCache, options);
        }

        public async Task AddItemToAnItemList<T1, T2>(
            T1 objToBeAdded,
            T2 item,
            string propertyName,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var listProperty = typeof(T2).GetProperty(propertyName);
            var list = (System.Collections.IList)listProperty.GetValue(item);
            list?.Add(objToBeAdded);
            await UpdateItemInCache(item, keyCache, options);
        }

        public async Task UpdateItemToAnItemList<T1, T2>(
            T1 objToBeUpdated,
            T2 item,
            string propertyName,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var listProperty = typeof(T2).GetProperty(propertyName);
            var list = (List<T1>)listProperty.GetValue(item);

            int indexItemToBeUpdated = list.FindIndex(x =>
                EqualityComparer<T1>.Default.Equals(x, objToBeUpdated)
                || (
                    typeof(T1)
                        .GetProperty("Id")
                        ?.GetValue(x)
                        ?.Equals(typeof(T1).GetProperty("Id")?.GetValue(objToBeUpdated)) ?? false
                )
            );

            if (indexItemToBeUpdated != -1)
            {
                list[indexItemToBeUpdated] = objToBeUpdated;
                await AddItemInCache<T2>(item, keyCache, options);
            }
        }

        public async Task RemoveItemByIdInAnItem<T, TListOfItemToBeRemoved>(
            Guid idToBeRemoved,
            string propertyName,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var item = await GetItemInCache<T>(keyCache);
            var listProperty = typeof(T).GetProperty(propertyName);
            var list = (List<TListOfItemToBeRemoved>)listProperty.GetValue(item);

            if (list?.Count == 1)
            {
                await RemoveItemInCache(keyCache);
            }
            else
            {
                list?.RemoveAll(x =>
                    (Guid)x.GetType().GetProperty("Id").GetValue(x) == idToBeRemoved
                );
                await UpdateItemInCache(item, keyCache, options);
            }
        }
    }
}
