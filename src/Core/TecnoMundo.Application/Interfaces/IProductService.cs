using Microsoft.Extensions.Caching.Distributed;
using TecnoMundo.Application.DTOs;

namespace TecnoMundo.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductVO>> FindAll(string keyCache, DistributedCacheEntryOptions options);
        Task<IEnumerable<CategoryVO>> FindAllCategories();
        Task<IEnumerable<ProductVO>> FindProductsByCategoryId(Guid id);
        Task<IEnumerable<ProductVO>> ProductFilter(
            string? name,
            decimal? priceOf,
            decimal? priceUpTo
        );
        Task<ProductVO?> FindById(Guid id, string keyCache, DistributedCacheEntryOptions options);
        Task<ProductVO> Create(CreateProductVO vo, string keyCache, DistributedCacheEntryOptions options);
        Task<ProductVO> Update(ProductVO vo, string keyCache, DistributedCacheEntryOptions options);
        Task<bool> Delete(Guid id, string keyCache, DistributedCacheEntryOptions options);
    }
}
