using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;

namespace GeekShopping.CartAPI.Repository
{
    public interface ICartRepoository
    {
        //recupera um carrinho de acordo com o id do usuário
        Task<CartVO> FindCartByUserId(string userId);
        Task<CartHeader> FindCartHeaderById(string id);
        Task<CartDetail> FindCartDetailByProductIdAndCartHeaderId(Guid productId, Guid cartHeaderId);
        Task AddCartDetails(CartDetail cartDetail);
        Task UpdateCartDetails(CartDetail cartDetail);
        Task AddCartHeaders(CartHeader cartHeader);
        Task<bool> RemoveFromCart(Guid cartDetailsId);
        Task<bool> ApplyCuopon(string userId, Guid couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
