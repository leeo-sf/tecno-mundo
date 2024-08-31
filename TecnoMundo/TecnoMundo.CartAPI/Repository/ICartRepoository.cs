using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;

namespace GeekShopping.CartAPI.Repository
{
    public interface ICartRepoository
    {
        //recupera um carrinho de acordo com o id do usuário
        Task<CartVO> FindCartByUserId(string userId);
        Task<CartHeader> FindCartHeaderById(string id);
        Task<CartDetail> FindCartDetailByProductIdAndCartHeaderId(long productId, long cartHeaderId);
        Task AddCartDetails(CartDetail cartDetail);
        Task UpdateCartDetails(CartDetail cartDetail);
        Task AddCartHeaders(CartHeader cartHeader);
        Task<bool> RemoveFromCart(long cartDetailsId);
        Task<bool> ApplyCuopon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
