using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repository
{
    public interface ICartRepoository
    {
        //recupera um carrinho de acordo com o id do usuário
        Task<CartVO> FindCartByUserId(string userId);
        Task<CartVO> SaveOrUpdateCart(CartVO cart, string token);
        Task<bool> RemoveFromCart(long cartDetailsId);
        Task<bool> ApplyCuopon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
