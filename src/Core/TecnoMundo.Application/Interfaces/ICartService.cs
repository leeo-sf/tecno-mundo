using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartVO> FindCartByUserId(Guid userId);
        Task<CartHeaderVO> FindCartHeaderById(Guid id);
        Task<CartDetailVO> FindCartDetailByProductIdAndCartHeaderId(
            Guid productId,
            Guid cartHeaderId
        );
        Task AddCartDetails(CartDetailVO vo);
        Task UpdateCartDetails(CartDetailVO vo);
        Task AddCartHeaders(CartHeaderVO vo);
        Task<bool> RemoveFromCart(Guid cartDetailsId);
        Task<bool> ApplyCoupon(Guid userId, string couponCode);
        Task<bool> RemoveCoupon(Guid userId);
        Task<bool> ClearCart(Guid userId);
        Task<CartVO> SaveOrUpdate(CartVO vo, ProductVO productVO);
    }
}
