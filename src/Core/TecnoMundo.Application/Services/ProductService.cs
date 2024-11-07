using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.ProductAPI.Caching;

namespace TecnoMundo.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ICachingService _cache;
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper, ICachingService cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<ProductVO>> FindAll(
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var products = await _cache.GetListCache<ProductVO>(keyCache);

            if (products.Count == 0)
            {
                var listProduct = await _repository.FindAll();
                var listProductVO = _mapper.Map<List<ProductVO>>(listProduct);
                products = await _cache.AddListInCache(listProductVO, keyCache, options);
            }

            return _mapper.Map<IEnumerable<ProductVO>>(products);
        }

        public async Task<IEnumerable<CategoryVO>> FindAllCategories()
        {
            var categories = await _repository.FindAllCategories();
            return _mapper.Map<IEnumerable<CategoryVO>>(categories);
        }

        public async Task<ProductVO?> FindById(
            Guid id,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var product = await _cache.GetItemInCache<ProductVO>(keyCache);

            if (product == null)
            {
                var productToBeAddCache = await _repository.FindById(id);

                if (productToBeAddCache == null)
                    return null;

                var productToBeAddCacheVO = _mapper.Map<ProductVO>(productToBeAddCache);
                product = await _cache.AddItemInCache(productToBeAddCacheVO, keyCache, options);
            }

            return _mapper.Map<ProductVO>(product);
        }

        public async Task<IEnumerable<ProductVO>> FindProductsByCategoryId(Guid id)
        {
            var products = await _repository.FindProductsByCategoryId(id);
            return _mapper.Map<IEnumerable<ProductVO>>(products);
        }

        public async Task<IEnumerable<ProductVO>> ProductFilter(
            string? name,
            decimal? priceOf,
            decimal? priceUpTo
        )
        {
            var products = await _repository.ProductFilter(name, priceOf, priceUpTo);
            return _mapper.Map<IEnumerable<ProductVO>>(products);
        }

        public async Task<ProductVO> Create(
            CreateProductVO vo,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var product = _mapper.Map<Product>(vo);
            var productCreated = await _repository.Create(product);
            var productCreatedVO = _mapper.Map<ProductVO>(productCreated);
            await _cache.AddItemToExistingListInCache(productCreatedVO, keyCache, options);
            return productCreatedVO;
        }

        public async Task<bool> Delete(
            Guid id,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var deleted = await _repository.Delete(id);
            if (deleted == null)
                return false;

            var productsInCache = await _cache.GetListCache<ProductVO>(keyCache);
            if (productsInCache.Count != 0)
            {
                var deletedVO = _mapper.Map<ProductVO>(deleted);
                await _cache.RemoveExistingListItemFromCache(
                    deletedVO,
                    productsInCache,
                    keyCache,
                    options
                );
                await _cache.RemoveItemInCache($"product-{id}");
            }

            return true;
        }

        public async Task<ProductVO> Update(
            ProductVO vo,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var product = _mapper.Map<Product>(vo);
            var productUpdated = await _repository.Update(product);

            var productsInCache = await _cache.GetListCache<ProductVO>(keyCache);
            if (productsInCache.Count != 0)
            {
                var productUpdatedVO = _mapper.Map<ProductVO>(productUpdated);
                await _cache.UpdateExistingListItemFromCache(
                    productUpdatedVO,
                    productsInCache,
                    keyCache,
                    options
                );
                await _cache.UpdateItemInCache(
                    productUpdatedVO,
                    $"product-{productUpdatedVO.Id}",
                    options
                );
            }

            return _mapper.Map<ProductVO>(productUpdated);
        }
    }
}
