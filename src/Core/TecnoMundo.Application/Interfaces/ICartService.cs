using Microsoft.Extensions.Caching.Distributed;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartVO?> FindCartByUserId(
            Guid userId,
            string keyCache,
            DistributedCacheEntryOptions options
        );
        Task<CartHeaderVO> FindCartHeaderById(Guid id);
        Task<CartDetailVO> FindCartDetailByProductIdAndCartHeaderId(
            Guid productId,
            Guid cartHeaderId
        );
        Task AddCartDetails(CartDetailVO vo);
        Task UpdateCartDetails(CartDetailVO vo);
        Task AddCartHeaders(CartHeaderVO vo);
        Task<bool> RemoveFromCart(
            Guid cartDetailsId,
            string keyCache,
            DistributedCacheEntryOptions options
        );
        Task<bool> ApplyCoupon(
            Guid userId,
            string couponCode,
            string keyCache,
            DistributedCacheEntryOptions options
        );
        Task<bool> RemoveCoupon(Guid userId, string keyCache, DistributedCacheEntryOptions options);
        Task<bool> ClearCart(Guid userId, string keyCache);
        Task<CartVO> SaveOrUpdate(
            CartVO vo,
            ProductVO productVO,
            string keyCache,
            DistributedCacheEntryOptions options
        );
        Task AddCartVOInCache(CartVO vo, string keyCache, DistributedCacheEntryOptions options);
    }
}
