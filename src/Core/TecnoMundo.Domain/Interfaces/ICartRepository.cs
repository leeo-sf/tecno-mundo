using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> FindCartByUserId(Guid userId);
        Task<CartHeader> FindCartHeaderById(Guid id);
        Task<CartDetail> FindCartDetailByProductIdAndCartHeaderId(
            Guid productId,
            Guid cartHeaderId
        );
        Task AddCartDetails(CartDetail cartDetail);
        Task UpdateCartDetails(CartDetail cartDetail);
        Task AddCartHeaders(CartHeader cartHeader);
        Task<bool> RemoveFromCart(Guid cartDetailsId);
        Task<bool> ApplyCoupon(Guid userId, string couponCode);
        Task<bool> RemoveCoupon(Guid userId);
        Task<bool> ClearCart(Guid userId);
    }
}
